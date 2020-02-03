using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContractsTracking.Utility.Models;
using System.Data;

namespace ContractsTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemLabelsController : ControllerBase
    {
        readonly Config config;

        public ProblemLabelsController()
        {
            config = new Config();
        }

        // GET: api/Problems
        [HttpGet]
        public IEnumerable<ProblemLabel> GetProblemItems()
        {
            List<ProblemLabel> problemLabelList = new List<ProblemLabel>();

            using (SqlConnection conn = new SqlConnection(config.connString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.ProblemLabels";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProblemLabel problemLabel = new ProblemLabel
                    {
                        problemName = reader.GetString(0),
                        defaultGroup = reader.GetString(1)
                    };
                    problemLabelList.Add(problemLabel);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }
            return problemLabelList.ToArray();
        }

        [HttpGet("{problemName}")]
        public List<ProblemLabel> GetProblemItem(string problemName)
        {
            List<ProblemLabel> problemList = new List<ProblemLabel>();

            using (SqlConnection conn = new SqlConnection(config.connString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.ProblemLabels WHERE problemName = '" + problemName + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProblemLabel problem = new ProblemLabel
                    {
                        problemName = reader.GetString(0),
                        defaultGroup = reader.GetString(1)
                    };
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
        public int PostProblem(ProblemLabel problem)
        {
            int rowsAffected = 0;
            if (!(problem is null))
            {
                using SqlConnection conn = new SqlConnection(config.connString);
                conn.Open();
                string sqlString = "UPDATE dbo.ProblemLabels " +
                                    "SET defaultGroup='" + problem.defaultGroup + "'" +
                                    "WHERE problemName LIKE '%" + problem.problemName + "%' ";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                rowsAffected = cmd.ExecuteNonQuery();


                cmd.Dispose();
                conn.Close();
            }
            return rowsAffected;
        }

        /*
        // DELETE: api/Problems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProblemLabel>> DeleteProblem(string id)
        {
            var problem = await _context.ProblemItems.FindAsync(id);
            if (problem == null)
            {
                return NotFound();
            }

            _context.ProblemItems.Remove(problem);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return problem;
        }

        private bool ProblemExists(string id)
        {
            return _context.ProblemItems.Any(e => e.problemName == id);
        }
        */
    }
}
