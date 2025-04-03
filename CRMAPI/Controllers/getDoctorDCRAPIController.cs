using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class getDoctorDCRAPIController : ControllerBase
    {
        private readonly string _connectionString;

        public getDoctorDCRAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }

        [HttpPost]
        [Route("Test_API")]
        public IActionResult EmpDetail(String name,String pwd)
        
        {
            var d = "";
            if(name =="Nabin" && pwd=="1234")
            {
                d = "Success";
            }
            return Ok(new { status = "d" });
        }


        [HttpGet("sp_getDoctorDCR")]
        public async Task<IActionResult> sp_getDoctorDCR()
        {
            List<doctorDCRDetails> doctors = new List<doctorDCRDetails>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getDoctorDCR", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            doctors.Add(new doctorDCRDetails
                            {
                                selectedArea = reader["selectedArea"].ToString(),
                                selectedDoctor = reader["selectedDoctor"].ToString(),
                                Visit_With = reader["Visit_With"].ToString(),
                                latitude = reader.GetDecimal(reader.GetOrdinal("latitude")),
                                longitute = reader.GetDecimal(reader.GetOrdinal("longitute"))
                            });
                        }
                    }
                }
            }

            return Ok(doctors);
        }
    }
}
