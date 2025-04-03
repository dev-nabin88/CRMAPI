using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RetailDCRAPIController : ControllerBase
    {
        private readonly string _connectionString;
        public RetailDCRAPIController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }

        [HttpPost(Name = "sp_AddRetailDCR")]
        public IActionResult sp_AddRetailDCR([FromBody] RetailDCRDetails retail)
        {
            if (retail == null)
            {
                return BadRequest("retail is required.");
            }

            using (SqlConnection conn = new(_connectionString))
            {
                SqlCommand cmd = new("sp_AddRetailDCR", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //cmd.Parameters.AddWithValue("@userID", user.userID);

                cmd.Parameters.AddWithValue("@selectedArea", retail.selectedArea);
                cmd.Parameters.AddWithValue("@selectedRetailer", retail.selectedRetailer);
                cmd.Parameters.AddWithValue("@Visit_With", retail.Visit_With);
                cmd.Parameters.AddWithValue("@latitude", retail.latitude);
                cmd.Parameters.AddWithValue("@longitude", retail.longitude);

                conn.Open();
                cmd.ExecuteNonQuery();

            }

            return Ok("RetailDCR added successfully!");
        }
    }

}
