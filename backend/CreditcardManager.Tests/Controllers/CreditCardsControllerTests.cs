using CreditCardManager.Models;
using CreditCardManager.Repositories;
using CreditCardManager.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace CreditCardManager.Controllers.Tests
{
    [TestClass()]
    public class CreditCardsControllerTests
    {
        private readonly CreditCardsController creditCardsController;
        private readonly CreditCardRepository creditCardRepository;

        public CreditCardsControllerTests()
        {
            var logger = new Mock<ILogger<CreditCardsController>>().Object;
            var protectionManager = new DataProtectionManager(new EphemeralDataProtectionProvider());
            var optionsBuilder = new DbContextOptionsBuilder<CreditCardRepository>().UseInMemoryDatabase("CreditCards");
            creditCardRepository = new CreditCardRepository(optionsBuilder.Options);
            var options = new MemoryCacheOptions();
            var memoryCache = new MemoryCache(options);
            creditCardsController = new CreditCardsController(memoryCache, logger, creditCardRepository, protectionManager);
        }

        [TestInitialize]
        public void SeedTestData()
        {
            creditCardRepository.RegisteredCards.AddRange([ new RegisteredCreditCard()
            {
                Id = Guid.Parse("254f1364-873c-4a11-ace4-4cab522f0942"),
                CardNumber = "378282246310005",
                CVC = "378",
                ExpiryDate = DateTime.UtcNow,
                Name = "TestCard001"
            } ]);
        }

        [TestMethod()]
        public async Task Given_ValidCardIdentifier_When_Fetched_Returns_MatchingResult()
        {
            Guid cardId = Guid.Parse("254f1364-873c-4a11-ace4-4cab522f0942");

            var actionResult = await creditCardsController.GetCreditCard(cardId);

            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.IsTrue(actionResult.Value.Id == cardId);
        }

        [TestMethod()]
        public async Task Given_InnvalidCardIdentifier_When_Fetched_Returns_NotFoundResult()
        {
            Guid cardId = Guid.Parse("6dcd2c10-08c9-4fcd-8bf0-0f9af2d6c0ce");

            var actionResult = await creditCardsController.GetCreditCard(cardId);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public async Task Given_ValidCardInput_When_Registered_Returns_SucessfulResult()
        {
            CreditCard creditCard = new()
            {
                Id = Guid.Parse("6dcd2c10-08c9-4fcd-8bf0-0f9af2d6c0ce"),
                CardNumber = "371449635398431",
                CVC = "371",
                ExpiryDate = DateTime.UtcNow,
                Name = "TestCard002"
            };

            var actionResult = await creditCardsController.PostCreditCard(creditCard);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var createdResult = (CreatedAtActionResult)actionResult.Result;
            Assert.IsTrue((createdResult.Value as RegisteredCreditCard)?.Id == creditCard.Id);
        }

        [TestMethod()]
        public async Task Given_ExistingCard_When_Registered_Returns_ConflictResult()
        {
            CreditCard creditCard = new()
            {
                Id = Guid.Parse("d51a8728-f2e6-49ce-a372-7d6618b30e89"),
                CardNumber = "371449635398431",
                CVC = "371",
                ExpiryDate = DateTime.UtcNow,
                Name = "TestCard002"
            };

            var actionResult = await creditCardsController.PostCreditCard(creditCard);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var createdResult = (CreatedAtActionResult)actionResult.Result;
            Assert.IsTrue((createdResult.Value as RegisteredCreditCard)?.Id == creditCard.Id);

            actionResult = await creditCardsController.PostCreditCard(creditCard);

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(ConflictResult));
        }
    }
}