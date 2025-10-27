using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SingerWebSiteIntegration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static SingerWebSiteIntegration.Models.DBConnection;

namespace SingerWebSiteIntegration.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class GetGRNDetailsController : ControllerBase
    {
        private DBConnection dbConnection = new DBConnection();
        public DBAccess dBAccess = new DBAccess();
        //private DataSet oDataSet;
        //private DataTable oDataTable = null;   
        //private DataRow O_dRow = null;
        [HttpGet("grndtl")]
        public  IActionResult GetGrnDtl([FromBody] GrnDetails grnDetails)
        {
            if (grnDetails.shipment_id == 0)
            {
               
                return Ok(JsonConvert.SerializeObject("Invalid shipment_id"));
            }
            else 
            {

                try
                {
                    OracleConnection oOracleConnection = dbConnection.GetConnection();
                    string devices_query = @"select h.shipment_id,
                                           h.ORDER_NO,
                                           ifsapp.Purchase_Order_Line_Part_API.Get_Part_No(ORDER_NO,LINE_NO,RELEASE_NO) Product_code,
                                           ifsapp.Purchase_Part_API.Get_Description(CONTRACT, ifsapp.Purchase_Order_Line_Part_API.Get_Part_No(ORDER_NO,LINE_NO,RELEASE_NO)) product_des,
                                           ifsapp.Purchase_Order_Line_API.Get_Buy_Qty_Due(h.ORDER_NO,h.LINE_NO,h.RELEASE_NO) qty
                                              from ifsapp.C_MULTI_SITE_SHIPMENT a, ifsapp.C_SHIPMENT_LINES h
                                             where h.shipment_id = a.shipment_id and a.STATE  IN ('Planned','Calculated')
                                             and a.shipment_id = '" + grnDetails.shipment_id + "'";
                    OracleCommand cmd_non_serial = new OracleCommand(devices_query, oOracleConnection);
                    OracleCommand cmd = new OracleCommand(devices_query, oOracleConnection);
                    OracleDataAdapter dataAdapter = new OracleDataAdapter();
                    dataAdapter.SelectCommand = cmd;
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                   
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }

                    return Ok(rows);
                   
                   
                    
                }
                catch (Exception ex)
                {
                    return Ok(new { record = 0, message = ex.Message });
                }
            }
            //end
            return Ok("UNSUCCESS");

        }


        [HttpGet("GetDebitNotePartList")]
        public IActionResult GetDebitNotePartList([FromQuery] string DebitNote, string Debit_site)
        {
            if (string.IsNullOrEmpty(DebitNote) || string.IsNullOrEmpty(Debit_site))
            {
                return BadRequest("Input parameters cannot be null or empty.");
            }

            try
            {
                DataTable db = dBAccess.GetDebitNotePart(DebitNote, Debit_site);

                if (db.Rows.Count > 0)
                {
                    var resultList = new List<object>();

                    foreach (DataRow row in db.Rows)
                    {
                        resultList.Add(new
                        {
                            debit_note_no = row["debit_note_no"].ToString(),
                            order_no = row["order_no"].ToString(),
                            contract = row["contract"].ToString(),
                            part_no = row["part_no"].ToString(),
                            QTY = row["QTY"].ToString()
                          

                        });
                    }

                    return Ok(new { response = 1, data = resultList });
                }
                else
                {
                    return Ok(new { response = 2, message = "Invalid Debit Number or Bulk Gate Pass NUmber" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { response = 3, message = "Server error", error = ex.Message });
            }
        }
    }
}

