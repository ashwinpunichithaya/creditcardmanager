using CreditCardManager.Models;
using Microsoft.AspNetCore.DataProtection;

namespace CreditCardManager.Security
{
    /// <summary>
    /// Data protection service to handle sensitive/PII data.
    /// </summary>
    public class DataProtectionManager
    {
        private readonly IDataProtector protector;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataProtectionManager(IDataProtectionProvider provider)
        {
            this.protector = provider.CreateProtector("DataProtectionManager");
        }

        /// <summary>
        /// Translates CreditCard entity to secure RegisteredCreditCard entity.
        /// </summary>
        /// <paramref name="creditCard">Credit card to secure</paramref>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public RegisteredCreditCard Protect(CreditCard creditCard)
        {
            if (creditCard == null)
            {
                throw new ArgumentNullException(nameof(creditCard));
            }
            else
            {
                return new RegisteredCreditCard
                {
                    CardNumber = protector.Protect(creditCard.CardNumber),
                    CVC = protector.Protect(creditCard.CVC),
                    Id = creditCard.Id,
                    ExpiryDate = creditCard.ExpiryDate,
                    Name = creditCard.Name
                };
            }
        }
    }
}
