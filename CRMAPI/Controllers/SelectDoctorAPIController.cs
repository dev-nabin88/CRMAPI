using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectDoctorAPIController : ControllerBase
    {
        private readonly string _connectionString;
        public SelectDoctorAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }

        [HttpGet("sp_GetDoctors")]
        public async Task<IActionResult> sp_GetDoctors(int areaId)
        {
            List<DoctorDetails> doctors = new List<DoctorDetails>();

            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDoctors", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@areaID", areaId);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            doctors.Add(new DoctorDetails
                            {
                                doctorID = reader.GetInt32(0),
                                doctorName = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return Ok(doctors);
        }


    }
}
