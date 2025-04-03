using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorDCRAPIController : ControllerBase
    {
        private readonly string _connectionString;
        public DoctorDCRAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }

        [HttpPost(Name = "sp_AddDoctorDCR")]
        public IActionResult sp_AddDoctorDCR([FromBody] doctorDCRDetails doctor)
        {
            if (doctor == null)
            {
                return BadRequest("doctor is required.");
            }

            using (SqlConnection conn = new(_connectionString))
            {
                SqlCommand cmd = new("sp_AddDoctorDCR", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //cmd.Parameters.AddWithValue("@userID", user.userID);

                cmd.Parameters.AddWithValue("@selectedArea", doctor.selectedArea);
                cmd.Parameters.AddWithValue("@selectedDoctor", doctor.selectedDoctor);
                cmd.Parameters.AddWithValue("@Visit_With", doctor.Visit_With);
                cmd.Parameters.AddWithValue("@latitude", doctor.latitude);
                cmd.Parameters.AddWithValue("@longitute", doctor.longitute);


                conn.Open();
                cmd.ExecuteNonQuery();

            }

            return Ok("doctorDCR added successfully!");
        }

    }

}
