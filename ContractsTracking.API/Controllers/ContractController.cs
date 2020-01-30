using Dapper;
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
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController
    {
        Config config;
        public ContractController()
        {
            config = new Config();
        }

        [HttpGet("{filterType}/{filterValue}")]
        public IEnumerable<Contract> Get(string filterType, string filterValue)
        {
            List<Contract> contractList = new List<Contract>();
            filterValue = String.IsNullOrWhiteSpace(filterValue) ? "" : filterValue;

            using (SqlConnection conn = new SqlConnection(config.connString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Contracts WHERE " + filterType + " LIKE '%" + filterValue + "%'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Contract contract = new Contract
                    {
                        customerName = reader.GetString(0),
                        status = reader.GetString(1),
                        contractNumber = reader.GetString(2),
                        dealerName = reader.GetString(3),
                        dealerNumber = reader.GetString(4),
                        dealerNotes = reader.GetString(5),
                        amountFinanced = reader.GetDecimal(6)
                    };
                    contractList.Add(contract);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }
            return contractList.ToArray();
        }
    }
}
