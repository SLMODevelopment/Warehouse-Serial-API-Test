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
    public class UploadGrnDtl : ControllerBase
    {
        private DBConnection dbConnection = new DBConnection();

        [HttpPost("UploadGrnDtl")]
        public IActionResult Insertgrndtl([FromBody] UploadGrn UploadGrn)
        {
            if (UploadGrn.shipment_id == 0)
            {
                return Ok(JsonConvert.SerializeObject("Invalid shipment_id"));
            }
            else
            {
                try
                {
                    using (OracleConnection oOracleConnection = dbConnection.GetConnection())
                    {
                        string checkExistenceQuery = @"
                        SELECT COUNT(*) 
                        FROM ifsapp.SIN_GRN_SERIAL_DTL 
                        WHERE SHIPMENT_ID = :shipment_id 
                          AND PRODUCT_CODE = :product_code 
                          
                          AND SERIAL_NO = :serial_no";

                        string insertQuery = @"
                        INSERT INTO ifsapp.SIN_GRN_SERIAL_DTL (SHIPMENT_ID, PRODUCT_CODE, SERIAL_NO, UPDATE_USER,CONTAINER_NO, STATUS)
                        VALUES (:shipment_id, :product_code, :serial_no, :update_user,:container_no, :status)";

                        int totalRowsAffected = 0;
                        List<string> failedSerials = new List<string>();

                        foreach (var serial in UploadGrn.serial_no)
                        {
                            bool serialExists = false;

                            using (OracleCommand checkCmd = new OracleCommand(checkExistenceQuery, oOracleConnection))
                            {
                                checkCmd.Parameters.Add(new OracleParameter("shipment_id", UploadGrn.shipment_id));
                                checkCmd.Parameters.Add(new OracleParameter("product_code", UploadGrn.product_code));
                                checkCmd.Parameters.Add(new OracleParameter("serial_no", serial));

                                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                                serialExists = (count > 0);
                            }

                            if (serialExists)
                            {
                                continue; // Skip this serial number as it already exists
                            }

                            using (OracleCommand insertCmd = new OracleCommand(insertQuery, oOracleConnection))
                            {
                                insertCmd.Parameters.Add(new OracleParameter("shipment_id", UploadGrn.shipment_id));
                                insertCmd.Parameters.Add(new OracleParameter("product_code", UploadGrn.product_code));
                                insertCmd.Parameters.Add(new OracleParameter("serial_no", serial));
                                insertCmd.Parameters.Add(new OracleParameter("update_user", UploadGrn.update_user));
                                insertCmd.Parameters.Add(new OracleParameter("container_no", UploadGrn.container_no));
                                insertCmd.Parameters.Add(new OracleParameter("status", UploadGrn.status));

                                int rowsAffected = insertCmd.ExecuteNonQuery();

                                if (rowsAffected <= 0)
                                {
                                    failedSerials.Add(serial);
                                }
                                else
                                {
                                    totalRowsAffected += rowsAffected;
                                }
                            }
                        }

                        if (failedSerials.Count > 0)
                        {
                            return Ok(new { response = 0, message = "Failed to insert data for serial numbers: " + string.Join(", ", failedSerials) });
                        }
                        else
                        {
                            return Ok(new { response = 1, message = "Data inserted successfully. Total rows affected: " + totalRowsAffected });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Ok(new { response = 0, message = ex.Message });
                }
            }
        }

        private IActionResult InternalServerError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
