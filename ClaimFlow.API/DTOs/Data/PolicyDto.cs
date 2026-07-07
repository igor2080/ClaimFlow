namespace ClaimFlow.API.DTOs.Data
{
    public class PolicyDto
    {
        public Guid PolicyId { get; set; }
        public int PolicyNumber { get; set; }
        public int PolicyType { get; set; }
        public int CoverageAmount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
