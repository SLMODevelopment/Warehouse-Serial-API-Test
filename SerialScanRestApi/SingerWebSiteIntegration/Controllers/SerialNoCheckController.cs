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

    public class SerialNoCheckController : ControllerBase
    {
        private DBConnection dbConnection = new DBConnection();
        //private DataSet oDataSet;
        //private DataTable oDataTable = null;   
        //private DataRow O_dRow = null;
        [HttpGet("SerialNo")]
        public IActionResult CheckSerialNo([FromBody] SerialNo serialNo)
        {
            if (serialNo.serial_no == "" && serialNo.part_no=="")
            {
                return Ok(JsonConvert.SerializeObject("Invalid serial No or part no"));
            }
            else 
            {
                try
                {
                    using (OracleConnection oOracleConnection = dbConnection.GetConnection())
                    {
                      

                        // Create the select query to check if the serial number exists
                        string devices_query = @"
                    
                                            SELECT serial_no
                                              FROM ifsapp.Serial_Trans_History h
                                             WHERE h.serial_no = :serial_no
                                               and
                                               and h.part_no = :part_no
                                            UNION ALL
                                            select J.SERIAL_NO
                                              from ifsapp.SIN_GRN_SERIAL_DTL J
                                             WHERE J.SERIAL_NO = :serial_no
                                               AND J.PRODUCT_CODE = :part_no";

                        // Create Oracle command
                        using (OracleCommand cmd = new OracleCommand(devices_query, oOracleConnection))
                        {
                            // Add parameter to prevent SQL injection
                            cmd.Parameters.Add(new OracleParameter("serial_no", serialNo.serial_no));
                            cmd.Parameters.Add(new OracleParameter("part_no", serialNo.part_no));
                            // Execute the select command
                            using (OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();
                                dataAdapter.Fill(dataTable);

                                // Check if any rows are returned
                                if (dataTable.Rows.Count > 0)
                                {
                                    // Serial number exists, return a message
                                    return Ok(new { record =1, message = "Serial number already available." });
                                }
                                else
                                {
                                    // Serial number does not exist, return an empty result
                                    return Ok(new { record = 0 ,message = "Serial number not found." });
                                }
                            }
                        }
                    }
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

