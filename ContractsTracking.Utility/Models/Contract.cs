using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractsTracking.Utility.Models
{
    public class Contract
    {
        public string customerName { get; set; }
        public string status { get; set; }
        public string contractNumber { get; set; }
        public string dealerName { get; set; }
        public string dealerNumber { get; set; }
        public string dealerNotes { get; set; }
        public decimal amountFinanced { get; set; }
    }
}
