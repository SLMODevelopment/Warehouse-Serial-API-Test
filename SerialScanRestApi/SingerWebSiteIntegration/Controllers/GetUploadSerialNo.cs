using CCD_Application_API.Models;
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

    public class GetUploadSerialNo : ControllerBase
    {
        private DBConnection dbConnection = new DBConnection();
        //private DataSet oDataSet;
        //private DataTable oDataTable = null;   
        //private DataRow O_dRow = null;
        [HttpGet("UploadSerialNo")]
        public IActionResult CheckSerialNo([FromBody] UploadSerialNo uploadSerialNo)
        {
            if (uploadSerialNo.shipment_id == 0 || uploadSerialNo.product_code=="" || uploadSerialNo.user_no=="")
            {
                return Unauthorized();
            }
            else 
            {
                try
                {
                    OracleConnection oOracleConnection = dbConnection.GetConnection();
                    string devices_query = @"select 
                                               g.serial_no
       
                                          from ifsapp.sin_grn_serial_dtl g
                                         where g.shipment_id = '"+uploadSerialNo.shipment_id+"' and g.product_code = '"+uploadSerialNo.product_code+"'  and g.status = 'Planned'  ";
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
                    // Handle exceptions
                    return Ok(new { record = 0, message = ex.Message });
                }
            }
            //end
            

        }

       
    }
}

