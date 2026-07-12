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
        public Claim(Guid claimId, Guid policyId, int amount, string description, DateTime incidentDate, DateTime createdAt)
        {
            ClaimId = claimId;
            PolicyId = policyId;
            Amount = amount;
            Description = description;
            IncidentDate = incidentDate;
            Status = ClaimStatus.UnderReview;
            CreatedAt = createdAt;

            var initialHistory = new ClaimStatusHistory
            {
                ClaimStatusHistoryId = Guid.NewGuid(),
                ClaimId = this.ClaimId,
                FromStatus = ClaimStatus.None,
                ToStatus = Status,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = "API",
                Comment = "Claim created"
            };

            StatusHistories.Add(initialHistory);
        }

        public Guid ClaimId { get; set; }
        public Guid PolicyId { get; set; }
        public Policy Policy { get; set; } = null!;
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncidentDate { get; set; }
        public ClaimStatus Status { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DecidedAt { get; private set; }
        public string? DecisionReason { get; private set; }
        public List<ClaimStatusHistory> StatusHistories { get; private set; } = [];

        public void UpdateStatus(ClaimStatus status, string changedBy, string comment = "")
        {
            if (this.Status == status) return; //redundant call

            ClaimStatus oldStatus = this.Status;
            this.Status = status;

            if (status == ClaimStatus.Approved || status == ClaimStatus.Rejected)
            {
                this.DecidedAt = DateTime.UtcNow;
                this.DecisionReason = comment;
            }

            var history = new ClaimStatusHistory
            {
                ClaimStatusHistoryId = Guid.NewGuid(),
                ClaimId = this.ClaimId,
                FromStatus = oldStatus,
                ToStatus = status,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = changedBy,
                Comment = comment
            };

            this.StatusHistories.Add(history);
        }
    }
}
