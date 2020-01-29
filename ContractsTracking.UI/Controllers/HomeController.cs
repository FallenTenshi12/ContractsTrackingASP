using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using ContractsTracking.Utility.Models;
using System.Web;
using System.Text;

namespace ContractsTracking.UI.Controllers
{
    public class HomeController : Controller
    {
        string baseURL = "https://localhost:44377/api/";

        /// <summary>
        /// Hub for the code for the Contract Data
        /// Please use await in the call as this is an async call
        /// </summary>
        /// <param name="urlExtention">This is everything in the url except the baseURL, as seen above</param>
        /// <returns>a list of contracts based on parameters</returns>
        private async Task<List<Contract>> GetContractData(string urlExtention)
        {
            List<Contract> contractList = new List<Contract>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "contract/" + urlExtention))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    contractList = JsonConvert.DeserializeObject<List<Contract>>(apiResponse);
                }
            }
            return contractList;
        }

        /// <summary>
        /// Hub for the code for the User Data
        /// Please use await in the call as this is an async call
        /// </summary>
        /// <param name="urlExtention">This is everything in the url except the baseURL, as seen above</param>
        /// <returns>a list of users based on parameters</returns>
        private async Task<List<User>> GetUserData(string urlExtention)
        {
            List<User> userList = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "user/" + urlExtention))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                }
            }
            return userList;
        }

        /// <summary>
        /// Hub for the code for the Problem Label Data
        /// Please use await in the call as this is an async call
        /// </summary>
        /// <param name="urlExtention">This is everything in the url except the baseURL, as seen above</param>
        /// <returns>a list of problem labels based on parameters</returns>
        private async Task<List<Problem>> GetProblemData(string urlExtention)
        {
            List<Problem> problemList = new List<Problem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "Problems/" + urlExtention))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    problemList = JsonConvert.DeserializeObject<List<Problem>>(apiResponse);
                }
            }
            return problemList;
        }

        private async Task<bool> SetProblemData(Problem problem)
        {
            bool isSuccessful = false;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(baseURL + "Problems/", problem.getJSON()))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSuccessful = true;
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + ": " + response.ReasonPhrase);
                    }
                }
            }
            return isSuccessful;
            /*
            List<Problem> problemList = new List<Problem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(baseURL + "Problems/" + urlExtention))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    problemList = JsonConvert.DeserializeObject<List<Problem>>(apiResponse);
                }
            }
            return problemList;
            */
        }

        public async Task<IActionResult> Index()
        {
            List<Contract> contractList = new List<Contract>();
            List<User> userResults = new List<User>();
            using (var httpClient = new HttpClient())
            {
                userResults = await GetUserData("steelea");

                string role = userResults[0].role.Equals("Contract Admin Manager") ? " " : userResults[0].role;

                contractList = await GetContractData("assignedGroup/" + role + "/");
            }
            return View(contractList);
        }

        public async Task<IActionResult> Contract(string contractID)
        {
            ViewData["Data"] = contractID;

            List<Contract> contractList = await GetContractData("contractNumber/" + contractID);
            return View(contractList);
        }

        public IActionResult Reporting()
        {
            return View();
        }

        public async Task<IActionResult> Settings()
        {
            List<Problem> problems = await GetProblemData("");
            return View(problems);
        }
        
        public async Task<IActionResult> UpdateProblemLabel(string problemID)
        {
            Problem problem = new Problem();
            problemID = problemID.Replace(" ", "%20", StringComparison.OrdinalIgnoreCase).Replace("/", "%2F", StringComparison.OrdinalIgnoreCase);
            List<Problem> problems = await GetProblemData(problemID);
            problem = problems[0];
            return View(problem);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProblem(string problemName, string defaultGroup, string location)
        {
            Problem problem = new Problem()
            {
                problemName = problemName,
                defaultGroup = defaultGroup,
                location = location
            };
            await SetProblemData(problem);
            //await GetProblemData("update/" + problemName + "/" + defaultGroup + "/" + location);
            return RedirectToAction("Settings");
        }

        public async Task<IActionResult> Search(string contractNumber)
        {
            List<Contract> contracts = new List<Contract>();
            if (contractNumber is null) contractNumber = new string(" /");
            contracts = await GetContractData("contractNumber/" + contractNumber);
            ViewData["Data"] = (contracts.Count != 0);
            return View(contracts);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
