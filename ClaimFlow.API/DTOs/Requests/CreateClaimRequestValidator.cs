using FluentValidation;

namespace ClaimFlow.API.DTOs.Requests
{
    public class CreateClaimRequestValidator : AbstractValidator<CreateClaimRequest>
    {
        public CreateClaimRequestValidator()
        {
            RuleFor(x => x.PolicyId)
                .NotEmpty().WithMessage("Policy ID is required.");
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Claim amount is required.")
                .GreaterThan(0).WithMessage("Amount must be positive");
            RuleFor(x => x.IncidentDate)
                .NotEmpty().WithMessage("Incident date is required.");
        }
    }
}
