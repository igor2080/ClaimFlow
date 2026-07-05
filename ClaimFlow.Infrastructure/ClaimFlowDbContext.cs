using ClaimFlow.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Infrastructure
{
    public class ClaimFlowDbContext(DbContextOptions<ClaimFlowDbContext> options) : DbContext(options)
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<ClaimStatusHistory> ClaimStatusHistories { get; set; }

    }
}
