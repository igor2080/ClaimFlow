using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClaimFlow.Domain
{
    public enum ClaimStatus
    {
        None = -1,
        UnderReview = 0,
        Approved = 1,
        Rejected = 2
    }
    public class Claim
    {
        private Claim()
        {

        }
        public Claim(Guid claimId, Guid policyId, int amount, string description, DateTime incidentDate, ClaimStatus status, DateTime createdAt, out ClaimStatusHistory initialHistory)
        {
            ClaimId = claimId;
            PolicyId = policyId;
            Amount = amount;
            Description = description;
            IncidentDate = incidentDate;
            Status = status;
            CreatedAt = createdAt;

            initialHistory = new ClaimStatusHistory
            {
                ClaimStatusHistoryId = Guid.NewGuid(),
                ClaimId = this.ClaimId,
                FromStatus = ClaimStatus.None,
                ToStatus = ClaimStatus.UnderReview,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "API",
                Comment = "Claim created"
            };
        }

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

        public ClaimStatusHistory UpdateStatus(ClaimStatus status, string changedBy, string comment = "")
        {
            ClaimStatus oldStatus = this.Status;
            this.Status = status;
            return new ClaimStatusHistory
            {
                ClaimStatusHistoryId = Guid.NewGuid(),
                ClaimId = this.ClaimId,
                FromStatus = oldStatus,
                ToStatus = status,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = changedBy,
                Comment = comment
            };
        }
    }
}
