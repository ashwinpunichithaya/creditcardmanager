namespace CreditCardManager.Models
{
    /// <summary>
    /// Business entity representing the credit card.
    /// </summary>
    public interface ICreditCard
    {
        /// <summary>
        /// The registered credit card number.
        /// </summary>
        string CardNumber { get; set; }

        /// <summary>
        /// The registered card verification code.
        /// </summary>
        string CVC { get; set; }

        /// <summary>
        /// The expiry date for the registered credit card.
        /// </summary>
        DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Unique identifier for registered credit card.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Name for the registered credit card.
        /// </summary>
        string Name { get; set; }
    }
}