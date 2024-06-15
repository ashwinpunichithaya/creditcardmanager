using CreditCardManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditCardManager.Repositories
{
    /// <summary>
    /// Data repository holding registered credit cards.
    /// </summary>
    public class CreditCardRepository : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public CreditCardRepository(DbContextOptions<CreditCardRepository> options) : base(options)
        {

        }

        /// <summary>
        /// Set of registered credit cards.
        /// </summary>
        public DbSet<RegisteredCreditCard> RegisteredCards { get; set; }
    }
}