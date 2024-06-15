using CreditCardManager.Validators;
using System.ComponentModel.DataAnnotations;

namespace CreditCardManager.Models
{
    /// <summary>
    /// Business entity representing the credit card.
    /// </summary>
    public class CreditCard : ICreditCard, IValidatableObject
    {
        /// <summary>
        /// The registered credit card number.
        /// </summary>
        [Required(ErrorMessage = "Card number is required")]
        [RegularExpression("[\\d]{14,16}", ErrorMessage = "Card number should between 14 to 16 digits")]
        public string CardNumber { get; set; }

        /// <summary>
        /// The registered card verification code.
        /// </summary>
        [Required(ErrorMessage = "CVC is required")]
        [RegularExpression("[\\d]{3}", ErrorMessage = "CVC should be 3-digit number")]
        public string CVC { get; set; }

        /// <summary>
        /// The expiry date for the registered credit card.
        /// </summary>
        [Required(ErrorMessage = "ExpiryDate is required")]
        [DataType(DataType.Date, ErrorMessage = "ExpiryDate should be a valid date")]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Unique identifier for registered credit card.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name for the registered credit card.
        /// </summary>
        [Required(ErrorMessage = "Card name is required")]
        [RegularExpression("[a-zA-Z0-9]{1,50}", ErrorMessage = "Card name should alphanumeric with maximum of 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Validates the entity model for any errors.
        /// </summary>
        /// <param name="validationContext">Context of the validation.</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var cardNumberValidator = validationContext.GetService<CardNumberValidator>();

            if (cardNumberValidator != null
                && !cardNumberValidator.IsValid(CardNumber))
            {
                yield return new ValidationResult("Credit card number is invalid", ["CardNumber"]);
            }
        }
    }
}