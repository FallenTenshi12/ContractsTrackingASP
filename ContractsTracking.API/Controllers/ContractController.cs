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
        private readonly string connectionString = @"Data Source=DESKTOP-JTHMVU3\SQLEXPRESS; Database=ContractsTracking; Integrated Security=true;";

        [HttpGet("{filterType}/{filterValue}")]
        public IEnumerable<Contract> Get(string filterType, string filterValue)
        {
            List<Contract> contractList = new List<Contract>();
            filterValue = String.IsNullOrWhiteSpace(filterValue) ? "" : filterValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Contracts WHERE " + filterType + " LIKE '%" + filterValue + "%'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Contract contract = new Contract();
                    contract.customerName = reader.GetString(0);
                    contract.status = reader.GetString(1);
                    contract.contractNumber = reader.GetString(2);
                    contract.dealerName = reader.GetString(3);
                    contract.dealerNumber = reader.GetString(4);
                    contract.dealerNotes = reader.GetString(5);
                    contract.amountFinanced = reader.GetDecimal(6);
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

/*
namespace ContractsTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {

        private readonly ILogger<ContractController> _logger;

        public ContractController(ILogger<ContractController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{filterType}/{filterValue}/")]
        public IEnumerable<Contract> Get(string filterType, string filterValue)
        {
            List<Contract> contractList = new List<Contract>();
            string connectionString = @"Data Source=DESKTOP-JTHMVU3\SQLEXPRESS; Database=ContractsTracking; Integrated Security=true";
            filterValue = String.IsNullOrWhiteSpace(filterValue) ? "" : filterValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Contracts WHERE " + filterType + " LIKE '%" + filterValue + "%'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Contract contract = new Contract();
                    contract.customerName = reader.GetString(0);
                    contract.status = reader.GetString(1);
                    contract.contractNumber = reader.GetString(2);
                    contract.dealerName = reader.GetString(3);
                    contract.dealerNumber = reader.GetString(4);
                    contract.dealerNotes = reader.GetString(5);
                    contract.amountFinanced = reader.GetDecimal(6);
                    contractList.Add(contract);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }
            return contractList.ToArray();
        }


        /*
        [HttpGet]
        public static IEnumerable<Contract> Get()
        {
            List<Contract> contractList = new List<Contract>();
            string connectionString = @"Data Source=DESKTOP-JTHMVU3\SQLEXPRESS; Database=ContractsTracking; Integrated Security=true";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Contracts";// WHERE assignedGroup = '" + role + "'";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    Contract contract = new Contract();
                    contract.customerName = reader.GetString(0);
                    contract.status = reader.GetString(1);
                    contract.contractNumber = reader.GetString(2);
                    contract.dealerName = reader.GetString(3);
                    contract.dealerNumber = reader.GetString(4);
                    contractList.Add(contract);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }

            return contractList.ToArray();
        }

        [HttpGet("{filterType}/{filterValue}")]
        public static IEnumerable<Contract> Get(string filterType, string filterValue)
        {
            List<Contract> contractList = new List<Contract>();
            string connectionString = @"Data Source=DESKTOP-JTHMVU3\SQLEXPRESS; Database=ContractsTracking; Integrated Security=true";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlString = "SELECT * FROM dbo.Contracts WHERE";
                if (filterType == "contractNumber")
                {
                    sqlString += "contractNumber = '" + filterValue + "'";
                } else
                {
                    sqlString += "assignedGroup = '" + filterValue + "'";
                }
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Contract contract = new Contract();
                    contract.customerName = reader.GetString(0);
                    contract.status = reader.GetString(1);
                    contract.contractNumber = reader.GetString(2);
                    contract.dealerName = reader.GetString(3);
                    contract.dealerNumber = reader.GetString(4);
                    contract.dealerNotes = reader.GetString(5);
                    contract.amountFinanced = reader.GetDecimal(6);
                    contractList.Add(contract);
                }

                reader.Close();
                cmd.Dispose();
                conn.Close();
            }
            return contractList;
        }
    }
}
*/
