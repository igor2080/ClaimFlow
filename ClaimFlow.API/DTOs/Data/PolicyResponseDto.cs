using ClaimFlow.Domain;

namespace ClaimFlow.API.DTOs.Data
{
    public class PolicyResponseDto
    {
        public Guid PolicyId { get; set; }
        public required CustomerDto Customer { get; set; }
        public int PolicyNumber { get; set; }
        public PolicyType PolicyType { get; set; }
        public int CoverageAmount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<ClaimDto>? Claims { get; set; }
        public List<ClaimHistoryDto>? ClaimStatusHistories { get; set; }

    }
}
