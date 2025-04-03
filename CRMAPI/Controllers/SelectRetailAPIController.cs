using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;


namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectRetailAPIController : ControllerBase
    {
        private readonly string _connectionString;
        public SelectRetailAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }

        [HttpGet("sp_GetRetailers")]
        public async Task<IActionResult> sp_GetRetailers(int areaId)
        {
            List<RetailDetails> retailers = new List<RetailDetails>();

            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetRetailers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@areaID", areaId);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            retailers.Add(new RetailDetails
                            {
                                retailerID = reader.GetInt32(0),
                                retailerName = reader.GetString(1)

                            });
                        }
                    }
                }
            }

            return Ok(retailers);
        }
    }

}
