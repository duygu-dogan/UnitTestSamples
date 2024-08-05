using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        private readonly IIdentityValidator _identityValidator;
        private const int minAge = 18;
        private List<string> techStackList = new()
        {"C#", "RabbitMQ", "Microservice", "Visual Studio"};
        private const int autoAcceptedExperience = 15;

        public ApplicationEvaluator(IIdentityValidator identityValidator)
        {
            _identityValidator = identityValidator;
        }

        public ApplicationResult Evaluate(JobApplication form)
        {
            if (form.Applicant is null)
                throw new ArgumentNullException();

            if(form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            _identityValidator.ValidationMode = form.Applicant.Age > 50 ? ValidationMode.Detailed : ValidationMode.Quick;

            if (_identityValidator.CountryDataProvider.CountryData.Country != "TURKEY")
                return ApplicationResult.TransferredToCTO;

            //var connectionSucceed = _identityValidator.CheckConnectionToRemoteServer();
            var validIdentity =  _identityValidator.IsValid(form.Applicant.IdentityNumber);

            if(!validIdentity)
                return ApplicationResult.TransferredToHr;

            var st = GetTechStackSimilartyRate(form.TechStackList);

            if (st < 25)
                return ApplicationResult.AutoRejected;

            if (st >= 75 && form.YearsOfExperience >= autoAcceptedExperience)
                return ApplicationResult.AutoAccepted;

            return ApplicationResult.AutoAccepted;
        }
        private int GetTechStackSimilartyRate(List<string> techStacks)
        {
            var matchedCount = techStacks.Where(x => techStackList.Contains(x, StringComparer.OrdinalIgnoreCase)).Count();
            return matchedCount * 100 / techStackList.Count;
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
