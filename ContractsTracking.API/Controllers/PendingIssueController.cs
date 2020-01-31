using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ContractsTracking.Utility.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractsTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingIssueController : ControllerBase
    {
        Config config;
        public PendingIssueController()
        {
            config = new Config();
        }

        [HttpGet("{filterType}/{filterValue}")]
        public IEnumerable<PendingIssue> Get(string filterType, string filterValue)
        {
            List<PendingIssue> pendingIssues = new List<PendingIssue>();
            using (SqlConnection conn = new SqlConnection(config.connString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.PendingIssues WHERE " + filterType + " LIKE '%" + filterValue + "%'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    PendingIssue issue = new PendingIssue
                    {
                        ContractNumber = reader.GetString(0),
                        Name = reader.GetString(1),
                        AssignedGroup = reader.GetString(2),
                        Status = reader.GetString(3),
                        TimeReported = reader.GetDateTime(4)
                    };
                    pendingIssues.Add(issue);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }
            return pendingIssues.ToArray();
        }

        // POST: api/PendingIssue
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/PendingIssue/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
