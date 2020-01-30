using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContractsTracking.Utility.Models
{
    public class PendingIssue
    {
        [Key]
        public string ContractNumber { get; set; }
        public string Name { get; set; }
        public string AssignedGroup { get; set; }
        public string Status { get; set; }
        public DateTime TimeReported { get; set; }
    }
}
