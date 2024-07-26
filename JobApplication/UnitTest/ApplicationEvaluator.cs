using JobApplicationLibrary.Models;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        private const int minAge = 18;
        public ApplicationResult Evaluate(JobApplication form)
        {
            if(form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            return ApplicationResult.AutoAccepted;
        }
        public enum ApplicationResult
        {
            AutoRejected,
            TransferredToHr,
            TransferredToLead,
            TransferredToCTO,
            AutoAccepted
        }

    }
}
