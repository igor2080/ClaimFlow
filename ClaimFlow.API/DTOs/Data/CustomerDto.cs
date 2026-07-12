namespace ClaimFlow.API.DTOs.Data
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PolicyDto>? Policies { get; set; }
    }
}
