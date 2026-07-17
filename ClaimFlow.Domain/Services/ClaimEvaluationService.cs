using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimFlow.Domain.Services
{
    public class ClaimEvaluationService
    {
        public void Evaluate(Claim claim, Policy policy)
        {
            if (claim == null || policy == null)
            {
                if (policy == null)
                    throw new ArgumentNullException("No policy was provided");
                else
                    throw new ArgumentNullException("No claim was provided");
            }
            if (claim.Status != ClaimStatus.UnderReview)
            {
                throw new InvalidOperationException("The claim has already been reviewed");
            }

            if (claim.Amount < policy.CoverageAmount)
            {
                claim.UpdateStatus(Domain.ClaimStatus.Approved, "Worker", "Fits within the coverage");
            }
            else
            {
                claim.UpdateStatus(Domain.ClaimStatus.Rejected, "Worker", "Outside the coverage range");
            }
        }
    }
}
