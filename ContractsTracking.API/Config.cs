using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractsTracking.API
{
    public class Config
    {
        public string connString { get; }

        public Config()
        {
            connString = @"Data Source=DESKTOP-JTHMVU3\SQLEXPRESS; Database=ContractsTracking; Integrated Security=true;";
        }
    }
}
