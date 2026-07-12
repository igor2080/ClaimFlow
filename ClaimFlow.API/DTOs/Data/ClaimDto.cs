using ClaimFlow.Domain;

namespace ClaimFlow.API.DTOs.Data
{
    public class ClaimDto
    {
        public Guid ClaimId { get; set; }
        public Guid? PolicyId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Status { get;  set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DecidedAt { get;  set; }
        public string? DecisionReason { get;  set; }
    }
}
