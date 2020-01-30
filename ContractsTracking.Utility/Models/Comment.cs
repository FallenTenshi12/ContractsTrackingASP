using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContractsTracking.Utility.Models
{
    public class Comment
    {
        [Key]
        public string contractNumber { get; set; }
        public string comment { get; set; }
        public string user { get; set; }
        public DateTime timeCommented { get; set; }
    }
}
