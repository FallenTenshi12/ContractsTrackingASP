using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContractsTracking.Utility.Models
{
    public class Problem
    {
        [Key]
        public string problemName { get; set; }
        public string defaultGroup { get; set; }
        public string location { get; set; }

        public StringContent getJSON()
        {
            string contents = JsonConvert.SerializeObject(this);
            var stringContent = new StringContent(contents, Encoding.UTF8, "application/json");
            return stringContent;
        }
    }
}
