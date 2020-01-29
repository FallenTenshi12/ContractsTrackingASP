using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContractsTracking.API.Models;
using ContractsTracking.Utility.Models;
using System.Data.SqlClient;
using Dapper;

namespace ContractsTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemsController : ControllerBase
    {
        private readonly ProblemContext _context;
        private readonly string connString = @"Data Source=DESKTOP-JTHMVU3\SQLEXPRESS; Database=ContractsTracking; Integrated Security=true";

        public ProblemsController(ProblemContext context)
        {
            _context = context;
        }

        // GET: api/Problems
        [HttpGet]
        public ActionResult<IEnumerable<Problem>> GetProblemItems()
        {
            List<Problem> problemList = new List<Problem>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Problems";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Problem problem = new Problem();
                    problem.problemName = reader.GetString(0);
                    problem.defaultGroup = reader.GetString(1);
                    problem.location = reader.GetString(2);
                    problemList.Add(problem);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }
            return problemList.ToArray();
        }

        // GET: api/Problems/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Problem>> GetProblem(string id)
        {
            List<Problem> problemList = new List<Problem>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Problems WHERE problemName LIKE '%" + id + "%'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Problem problem = new Problem();
                    problem.problemName = reader.GetString(0);
                    problem.defaultGroup = reader.GetString(1);
                    problem.location = reader.GetString(2);
                    problemList.Add(problem);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }
            return problemList;
        }

        // POST: api/Problems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public int PostProblem(Problem problem)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sqlString = "UPDATE dbo.Problems " +
                                    "SET defaultGroup='" + problem.defaultGroup + "', location='" + problem.location + "' " +
                                    "WHERE problemName LIKE '%" + problem.problemName + "%' ";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                rowsAffected = cmd.ExecuteNonQuery();


                cmd.Dispose();
                conn.Close();
            }
            return rowsAffected;
        }

        // DELETE: api/Problems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Problem>> DeleteProblem(string id)
        {
            var problem = await _context.ProblemItems.FindAsync(id);
            if (problem == null)
            {
                return NotFound();
            }

            _context.ProblemItems.Remove(problem);
            await _context.SaveChangesAsync();

            return problem;
        }

        private bool ProblemExists(string id)
        {
            return _context.ProblemItems.Any(e => e.problemName == id);
        }
    }
}
