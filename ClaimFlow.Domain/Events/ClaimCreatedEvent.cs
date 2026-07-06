using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Domain.Events
{
    public record ClaimCreatedEvent(
        Guid ClaimId,
        Guid PolicyId,
        int Amount,
        string Description,
        DateTime IncidentDate,
        DateTime CreatedAt,
        ClaimStatus Status
    );

}
