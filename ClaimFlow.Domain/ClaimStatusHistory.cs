using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Domain
{
    public class ClaimStatusHistory
    {
        public Guid ClaimStatusHistoryId { get; set; }
        public Guid ClaimId { get; set; }
        public Claim Claim { get; set; }
        public ClaimStatus FromStatus { get; set; }
        public ClaimStatus ToStatus { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; }
        public string Comment { get; set; }


    }
}
