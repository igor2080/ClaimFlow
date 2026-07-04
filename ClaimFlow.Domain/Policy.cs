using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Domain
{
    public enum PolicyType
    {
        Auto,
        Property,
        Health
    }
    public class Policy
    {
        public int PolicyId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int PolicyNumber { get; set; }
        public PolicyType Type { get; set; }
        public int CoverageAmount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
