using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CRMAPI.Controllers
{
    public class LoginValidationController : ControllerBase
    {
        private readonly string _connectionString;
        public LoginValidationController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CRMAPI");
        }

        [HttpPost("sp_UsersAuth")]
        public IActionResult sp_UsersAuth([FromBody] UserLogin user)
        {
            if (user == null || string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.userpassword))
            {
                return BadRequest("Username and password are required.");
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UsersAuth", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 100) { Value = user.username });
                cmd.Parameters.Add(new SqlParameter("@userpassword", SqlDbType.VarChar, 100) { Value = user.userpassword });

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Ok(new { message = "Login successful", username = result.ToString() });
                }
                else
                {
                    return Unauthorized("Invalid username or password.");
                }
            }
        }



    }

}
