using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectAreaAPIController : ControllerBase
    {

        private readonly string _connectionString;
        public SelectAreaAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }


        [HttpGet(Name = "sp_GetAreas")]
        public IActionResult sp_GetAreas()
        {
            List<AreaDetails> users = new();


            using (SqlConnection conn = new(_connectionString))
            {
                SqlCommand cmd = new("sp_GetAreas", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new AreaDetails
                        {
                            areaID = reader.GetInt32(0),
                            areaName = reader.GetString(1),


                        });
                    }
                }
            }

            if (users.Count == 0)
                return NotFound("No area found.");

            return Ok(users);


        }
    }
}
