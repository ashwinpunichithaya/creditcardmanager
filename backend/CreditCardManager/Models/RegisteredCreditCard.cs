using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CreditCardManager.Models
{
    /// <summary>
    /// Business entity representing the registered credit card.
    /// </summary>
    public class RegisteredCreditCard : ICreditCard
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [JsonIgnore]
        public string CardNumber { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [JsonIgnore]
        public string CVC { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name { get; set; }
    }
}