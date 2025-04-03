using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class getRetailDCRAPIController : ControllerBase
    {
        private readonly string _connectionString;

        public getRetailDCRAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }

        [HttpGet("sp_getRetailDCR")]
        public async Task<IActionResult> sp_getRetailDCR()
        {
            List<RetailDCRDetails> retailers = new List<RetailDCRDetails>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getRetailDCR", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            retailers.Add(new RetailDCRDetails
                            {
                                selectedArea = reader["selectedArea"].ToString(),
                                selectedRetailer = reader["selectedRetailer"].ToString(),
                                Visit_With = reader["Visit_With"].ToString(),
                                latitude = reader.GetDecimal(reader.GetOrdinal("latitude")),
                                longitude = reader.GetDecimal(reader.GetOrdinal("longitude"))
                            });
                        }
                    }
                }
            }

            return Ok(retailers);
        }


    }
}
