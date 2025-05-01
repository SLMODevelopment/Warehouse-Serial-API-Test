using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SingerWebSiteIntegration.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using static SingerWebSiteIntegration.Models.DBConnection;

namespace SingerWebSiteIntegration.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteShipmentController : ControllerBase
    {
        private readonly DBConnection dbConnection = new DBConnection();

        [HttpPost("DeleteShipment")] // Changed from GET to POST since you're sending a request body
        public IActionResult DeleteShipment([FromBody] ContainerList containerList)
        {
            if (containerList == null || containerList.shipment_id == 0)
            {
                return BadRequest(JsonConvert.SerializeObject("Invalid shipment_id"));
            }

            try
            {
                using (OracleConnection oOracleConnection = dbConnection.GetConnection())
                {
                   
                    string deleteQuery = @"DELETE FROM ifsapp.SIN_GRN_SERIAL_DTL WHERE SHIPMENT_ID = :shipment_id";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, oOracleConnection))
                    {
                        cmd.Parameters.Add(new OracleParameter("shipment_id", containerList.shipment_id));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok(new { response = rowsAffected, message = "Shipment deleted successfully" });
                        }
                        else
                        {
                            return NotFound(new { response = 0, message = "No matching shipment found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { response = 0, message = ex.Message });
            }
        }
        [HttpPost("DeleteShipmentSerialNo")]
        [HttpPost]
        public IActionResult DeleteSerialNo([FromBody] SerialNoDelete SerialNo)
        {
            if (string.IsNullOrWhiteSpace(SerialNo.serialno) || SerialNo.shipment_id == 0)
            {
                return BadRequest(new { response = 0, message = "Invalid shipment_id or serial_no" });
            }

            try
            {
                using (OracleConnection oOracleConnection = dbConnection.GetConnection())
                {
                    string deleteQuery = @"
                DELETE FROM ifsapp.SIN_GRN_SERIAL_DTL 
                WHERE SHIPMENT_ID = :shipment_id AND SERIAL_NO = :serial_no";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, oOracleConnection))
                    {
                        cmd.Parameters.Add(new OracleParameter("shipment_id", SerialNo.shipment_id));
                        cmd.Parameters.Add(new OracleParameter("serial_no", SerialNo.serialno));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok(new { response = rowsAffected, message = "Serial No deleted successfully" });
                        }
                        else
                        {
                            return NotFound(new { response = 0, message = "No matching Serial No found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { response = 0, message = ex.Message });
            }
        }

    }
}
