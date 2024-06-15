using CreditCardManager.Validators;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace CreditCardManager.Models.Tests
{
    [TestClass()]
    public class CreditCardTests
    {
        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Init()
        {
            var mockService = new Mock<IServiceProvider>();
            var mockCardNumberValidatorService = new Mock<CardNumberValidator>();
            mockService.Setup(x => x.GetService(typeof(CardNumberValidator)))
                       .Returns(mockCardNumberValidatorService.Object);
            serviceProvider = mockService.Object;
        }

        [TestMethod()]
        [DataRow("AmericanExpress", "123456789101112", "123")]
        [DataRow("HSBC", "371449635398431", "Junk")]
        [DataRow("$$Credit", "371449635398431", "371")]
        [DataRow("bsirpgnqwiwgnpithnhbgnnhwgeapomiacyoejguszziofurapt", "371449635398431", "371")]
        public void Given_InvalidCreditCardData_When_Validated_Then_Returns_ValidationErrors(string cardName, string cardNumber, string cvc)
        {
            CreditCard creditCard = new()
            {
                CardNumber = cardNumber,
                CVC = cvc,
                ExpiryDate = DateTime.UtcNow,
                Name = cardName
            };

            var validationResults = new List<ValidationResult>();

            ValidationContext validationContext = new(instance: creditCard, serviceProvider: serviceProvider, items: null);

            Validator.TryValidateObject(creditCard, validationContext, validationResults, true);

            Assert.IsTrue(validationResults.Count > 0);
        }

        [TestMethod()]
        public void Given_ValidCreditCardData_When_Validated_Then_Returns_No_ValidationErrors()
        {
            CreditCard creditCard = new()
            {
                CardNumber = "371449635398431",
                CVC = "371",
                ExpiryDate = DateTime.UtcNow,
                Name = "AmericanExpress"
            };

            var validationResults = new List<ValidationResult>();

            ValidationContext validationContext = new(instance: creditCard, serviceProvider: serviceProvider, items: null);

            Validator.TryValidateObject(creditCard, validationContext, validationResults, true);

            Assert.IsTrue(validationResults.Count() == 0);
        }
    }
}