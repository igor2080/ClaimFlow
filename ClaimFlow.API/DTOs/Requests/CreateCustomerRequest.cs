using System.ComponentModel.DataAnnotations;

namespace ClaimFlow.API.DTOs.Requests
{
    public record CreateCustomerRequest(
        string FullName,
        string Email

    );

}
