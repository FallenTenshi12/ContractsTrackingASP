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
        public int Add(PendingIssue issue)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(config.connString))
            {
                conn.Open();
                string sqlString = "INSERT INTO dbo.PendingIssues (contractNumber, name, assignedGroup, status, timeReported) VALUES " +
                                        "('"+issue.ContractNumber+"','"+issue.Name+"','"+issue.AssignedGroup+"','"+issue.Status+"','"+issue.TimeReported+"')";

                SqlCommand cmd = new SqlCommand(sqlString, conn);
                rowsAffected = cmd.ExecuteNonQuery();


                cmd.Dispose();
                conn.Close();
            }
            return rowsAffected;
        }

        [HttpPost("update")]
        public int Update(PendingIssue issue)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(config.connString))
            {
                conn.Open();
                string sqlString = "UPDATE dbo.PendingIssues " +
                                    "SET status='" + issue.Status + "' " +
                                    "WHERE name LIKE '%" + issue.Name + "%' AND contractNumber LIKE '%" + issue.ContractNumber + "%'";

                SqlCommand cmd = new SqlCommand(sqlString, conn);
                rowsAffected = cmd.ExecuteNonQuery();


                cmd.Dispose();
                conn.Close();
            }
            return rowsAffected;
        }
    }
}
