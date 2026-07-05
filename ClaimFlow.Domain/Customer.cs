using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Domain
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Policy> Policies { get; set; }

    }
}
