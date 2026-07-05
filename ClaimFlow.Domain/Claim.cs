using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Domain
{
    public enum ClaimStatus
    {
        UnderReview,
        Approved,
        Rejected
    }
    public class Claim
    {
        public Guid ClaimId { get; set; }
        public Guid PolicyId { get; set; }
        public Policy Policy { get; set; } = null!;
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncidentDate { get; set; }
        public ClaimStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DecidedAt { get; set; }
        public string? DecisionReason { get; set; }

    }
}
