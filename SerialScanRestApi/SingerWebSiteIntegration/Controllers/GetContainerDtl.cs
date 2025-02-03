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

    public class GetContainerDtl : ControllerBase
    {
        private DBConnection dbConnection = new DBConnection();
        //private DataSet oDataSet;
        //private DataTable oDataTable = null;   
        //private DataRow O_dRow = null;
        [HttpGet("grndtl")]
        public  IActionResult getContainerList([FromBody] ContainerList  containerList)
        {
            if (containerList.shipment_id == 0)
            {
               
                return Ok(JsonConvert.SerializeObject("Invalid shipment_id"));
            }
            else 
            {

                try
                {
                    OracleConnection oOracleConnection = dbConnection.GetConnection();
                    string devices_query = @"select G.CF$_X_CONTAINER_NO Container_No
                                              from ifsapp.X_MULTI_SITE_CONTAINER_CLV G
                                             where G.CF$_X_SHIPNMENT_ID = '" + containerList.shipment_id + "'";
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

