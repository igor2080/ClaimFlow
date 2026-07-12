using ClaimFlow.Domain;

namespace ClaimFlow.API.DTOs.Data
{
    public class ClaimHistoryDto
    {
        public Guid ClaimStatusHistoryId { get; set; }
        public Guid ClaimId { get; set; }
        public string FromStatus { get; set; }
        public string ToStatus { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; }
        public string Comment { get; set; }
    }
}
