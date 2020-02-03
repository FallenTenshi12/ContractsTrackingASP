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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContractsTracking.UI.Controllers
{
    public class ContractData
    {
        public Contract Contract { get; set; }
        public User User { get; set; }
    }

    public class HomeController : Controller
    {
        readonly string baseURL = "https://localhost:44377/api/";

#region APICalls
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
                using var response = await httpClient.GetAsync(baseURL + "contract/" + urlExtention);
                string apiResponse = await response.Content.ReadAsStringAsync();
                contractList = JsonConvert.DeserializeObject<List<Contract>>(apiResponse);
            }
            return contractList;
        }

        /// <summary>
        /// Hub for the code for the User Data
        /// Please use await in the call as this is an async call
        /// </summary>
        /// <param name="urlExtention">This is everything in the url except the baseURL, as seen above</param>
        /// <returns>a list of users based on parameters</returns>
        private async Task<User> GetUserData(string urlExtention)
        {
            List<User> userList = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(baseURL + "user/" + urlExtention);
                string apiResponse = await response.Content.ReadAsStringAsync();
                userList = JsonConvert.DeserializeObject<List<User>>(apiResponse);
            }
            return userList[0];
        }

        /// <summary>
        /// Hub for the code for the Problem Label Data
        /// Please use await in the call as this is an async call
        /// </summary>
        /// <param name="urlExtention">This is everything in the url except the baseURL, as seen above</param>
        /// <returns>a list of problem labels based on parameters</returns>
        private async Task<List<ProblemLabel>> GetProblemData(string urlExtention)
        {
            List<ProblemLabel> problemLabelList = new List<ProblemLabel>();
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync(baseURL + "ProblemLabels/" + urlExtention);
                string apiResponse = await response.Content.ReadAsStringAsync();
                problemLabelList = JsonConvert.DeserializeObject<List<ProblemLabel>>(apiResponse);
            }
            return problemLabelList;
        }

        private async Task<bool> SetProblemData(ProblemLabel problem)
        {
            bool isSuccessful = false;
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.PostAsync(baseURL + "ProblemLabels/", problem.getJSON());
                if (response.IsSuccessStatusCode)
                {
                    isSuccessful = true;
                }
                else
                {
                    Console.WriteLine(response.StatusCode + ": " + response.ReasonPhrase);
                }
            }
            return isSuccessful;
        }

        private async Task<List<PendingIssue>> GetPendingIssues(string urlExtention)
        {
            List<PendingIssue> pendingIssues = new List<PendingIssue>();
            using (var httpClient = new HttpClient())
            {
                string url = baseURL + "PendingIssue/" + urlExtention;
                using var response = await httpClient.GetAsync(url);
                string apiResponse = await response.Content.ReadAsStringAsync();
                pendingIssues = JsonConvert.DeserializeObject<List<PendingIssue>>(apiResponse);
            }
            return pendingIssues;
        }

        private async Task<bool> AddPendingIssue(PendingIssue issue)
        {
            bool isSuccessful = false;
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.PostAsync(baseURL + "PendingIssue/", issue.getJSON());
                if (response.IsSuccessStatusCode)
                    isSuccessful = true;
            }
            return isSuccessful;
        }

        private async Task<bool> UpdatePendingIssue(PendingIssue issue)
        {
            bool isSuccessful = false;
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.PostAsync(baseURL + "PendingIssue/update", issue.getJSON());
                if (response.IsSuccessStatusCode)
                    isSuccessful = true;
            }
            return isSuccessful;
        }
#endregion APICalls

#region UI Elements
        public IActionResult Index()
        {
            return View("Index");
        }

        public async Task<IActionResult> Dashboard(string username, string password)
        {
            if (!(username is null) && !(password is null))
            {
                User user = await GetUserData(username);
                if (user is null)
                {
                    return RedirectToAction("Index");
                }
                List<Contract> contractList = await GetContractData("assignedUser/" + user.username + "/");
                ViewData["user"] = user.username;
                return View(contractList);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Contract(string contractID, string username)
        {
            ViewData["Data"] = contractID;
            ContractData data = new ContractData();
            User user = await GetUserData(username);

            List<Contract> contractList = await GetContractData("contractNumber/" + contractID);
            Contract contract = contractList[0];
            contract.pendingIssues = await GetPendingIssues("contractNumber/" + contractID);
            data.Contract = contract;
            data.User = user;
            return View(data);
        }

        public async Task<IActionResult> AddPendingIssueView(string contractNumber)
        {
            List<ProblemLabel> problems = await GetProblemData("");
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (ProblemLabel label in problems)
            {
                items.Add(new SelectListItem { Value = label.problemName, Text = label.problemName });
            }
            ViewData["contractNum"] = contractNumber;
            return View(items);
        }

        public async Task<IActionResult> AddPendingIssue(string problemName, string contractNumber)
        {
            ProblemLabel issue = (await GetProblemData(problemName))[0];
            PendingIssue newIssue = new PendingIssue()
            {
                ContractNumber = contractNumber,
                Name = problemName,
                AssignedGroup = issue.defaultGroup,
                Status = "New",
                TimeReported = DateTime.Now
            };
            await AddPendingIssue(newIssue);
            return RedirectToAction("Contract", new { contractID = contractNumber });
        }

        public async Task<IActionResult> UpdatePendingIssueView(string problemName, string contractNumber)
        {
            List<PendingIssue> issues = await GetPendingIssues("contractNumber/" + contractNumber);
            PendingIssue issue = new PendingIssue();
            foreach (PendingIssue tmp in issues)
            {
                if (tmp.Name.Equals(problemName))
                {
                    issue = tmp;
                }
            }

            return View(issue);
        }

        public async Task<IActionResult> UpdatePendingIssue(string name, string contractNumber, string status)
        {
            PendingIssue issue = new PendingIssue() {
                Name = name,
                ContractNumber = contractNumber,
                Status = status
            };
            await UpdatePendingIssue(issue);
            return RedirectToAction("Contract", new { contractID = contractNumber, username = "goodwinn" });
        }

        public async Task<IActionResult> Workbasket()
        {
            List<Contract> contractList = new List<Contract>();
            User user = await GetUserData("meinersj");
            if (user.role.Contains("Contract Admin"))
                user.role = " ";
            List<PendingIssue> pendingIssues = await GetPendingIssues("assignedGroup/" + user.role + "/");
            foreach (PendingIssue issue in pendingIssues)
            {
                Contract tempContract = (await GetContractData("contractNumber/" + issue.ContractNumber))[0];
                bool found = false;
                foreach (Contract contract in contractList)
                {
                    if (contract.contractNumber.Equals(tempContract.contractNumber))
                        found = true;
                }
                if (!found)
                    contractList.Add(tempContract);
            }
            return View(contractList);
        }

        public IActionResult Reporting()
        {
            return View();
        }

        public async Task<IActionResult> Settings()
        {
            List<ProblemLabel> problems = await GetProblemData("");
            return View(problems);
        }
        
        public async Task<IActionResult> UpdateProblemLabel(string problemID)
        {
            problemID = HttpUtility.UrlEncode(problemID);
            List<ProblemLabel> problemLabels = await GetProblemData(problemID);
            var problemLabel = problemLabels[0];
            return View(problemLabel);
        }

        public async Task<IActionResult> UpdateProblem(string problemName, string defaultGroup)
        {
            ProblemLabel problem = new ProblemLabel()
            {
                problemName = problemName,
                defaultGroup = defaultGroup
            };
            await SetProblemData(problem);
            //await GetProblemData("update/" + problemName + "/" + defaultGroup + "/" + location);
            return RedirectToAction("Settings");
        }

        public async Task<IActionResult> Search(string contractNumber)
        {
            if (contractNumber is null) contractNumber = new string(" /");
            List<Contract> contracts = await GetContractData("contractNumber/" + contractNumber);
            ViewData["Data"] = (contracts.Count != 0);
            return View(contracts);
        }
#endregion UI Elements


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
