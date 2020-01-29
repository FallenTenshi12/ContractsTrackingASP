using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContractsTracking.Utility.Models;

namespace ContractsTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{username}")]
        public IEnumerable<User> Get(string username)
        {
            List<User> userResult = new List<User>();
            string connectionString = @"Data Source=DESKTOP-JTHMVU3\SQLEXPRESS; Database=ContractsTracking; Integrated Security=true";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Users WHERE username = '" + username + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    User user = new User();
                    user.displayName = reader.GetString(0);
                    user.role = reader.GetString(1);
                    userResult.Add(user);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            return userResult.ToArray();
        }
    }
}
