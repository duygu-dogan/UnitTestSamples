using JobApplicationLibrary.Models;
using JobApplicationLibrary;
using static JobApplicationLibrary.ApplicationEvaluator;
using Moq;
using JobApplicationLibrary.Services;

namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluationUnitTest
    {
        //UnitOfWork_Condition_ExpectedResult
        [Test]
        public void Application_WithUnderAge_TransferredToAutoRejected()
        {
            //Arrange
            var evaluator = new ApplicationEvaluator(null);
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 17
                }
            };

            //Action
         
            var appResult = evaluator.Evaluate(form);

            //Assert
            Assert.AreEqual(appResult, ApplicationResult.AutoRejected);
            
        }

        [Test]
        public void Application_WithNoSkill_TransferredToAutoRejected()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();

            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");

            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);

            var evaluator = new ApplicationEvaluator(mockValidator.Object);

            var appForm = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 25
                },
                TechStackList = new List<string>()
                { "" }
            };

            //Action
            var appResult = evaluator.Evaluate(appForm);

            //Assert
            Assert.AreEqual(appResult,ApplicationResult.AutoRejected);
        }

        [Test]
        public void Application_WithSkillOver75AndExperienceOver15_TransferredToAutoAccepted()
        {
            var mockValidator = new Mock<IIdentityValidator>();

            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");

            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);

            var evaluator = new ApplicationEvaluator(mockValidator.Object);

            var application = new JobApplication()
            {
                Applicant = new Applicant(),
                TechStackList = new List<string>() { "C#", "RabbitMQ", "Microservice", "Visual Studio" },
                YearsOfExperience = 15
            };

            var appResult = evaluator.Evaluate(application);

            Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoAccepted));
        }

        [Test]
        public void Application_WithInvalidIdentityNum_TransferredToHR()
        {
            var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Loose);

            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKEY");

            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);

            var evaluator = new ApplicationEvaluator(mockValidator.Object);

            var application = new JobApplication()
            {
                Applicant = new Applicant(),
            };

            var appResult = evaluator.Evaluate(application);

            Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToHr));
        }

        [Test]
        public void Application_WithOfficeLocation_TransferredToCTO()
        {
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("SPAIN");
            
            var evaluator = new ApplicationEvaluator(mockValidator.Object);

            var application = new JobApplication()
            {
                Applicant = new Applicant()
            };

            var appResult = evaluator.Evaluate(application);

            Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToCTO));
        }
    }
}