using System.ComponentModel.DataAnnotations;

namespace ClaimFlow.API.DTOs.Requests
{
    public record CreatePolicyRequest(
        [Required]
        Guid CustomerId,

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Policy number must be greater than 0")]
        int PolicyNumber,

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Policy type must be a valid enum integer.")]
        int PolicyType,

        [Required]
        [Range(1, 10_000_000, ErrorMessage = "Coverage amount must be between 1 and 10,000,000.")]
        int CoverageAmount,

        [Required]
        DateTime ValidFrom,

        [Required]
        DateTime ValidTo
    );
}
