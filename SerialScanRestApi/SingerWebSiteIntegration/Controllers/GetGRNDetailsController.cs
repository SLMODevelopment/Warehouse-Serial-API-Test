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


    }
}

