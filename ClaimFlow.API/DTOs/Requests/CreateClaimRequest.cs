using System.ComponentModel.DataAnnotations;

namespace ClaimFlow.API.DTOs.Requests
{
    public record CreateClaimRequest(
            Guid PolicyId,
            int Amount,
            string Description,
            DateTime IncidentDate      
        
        );
    
}
