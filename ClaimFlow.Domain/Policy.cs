using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Domain
{
    public enum PolicyType
    {
        Auto = 0,
        Property = 1,
        Health = 2,
    }
    public class Policy
    {
        public Guid PolicyId { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int PolicyNumber { get; set; }
        public PolicyType PolicyType { get; set; }
        public int CoverageAmount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
