using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContractsTracking.Utility.Models;

namespace ContractsTracking.API.Models
{
    public class ProblemContext : DbContext
    {
        public ProblemContext(DbContextOptions<ProblemContext> options) : base(options)
        {

        }

        public DbSet<Problem> ProblemItems { get; set; }
    }
}
