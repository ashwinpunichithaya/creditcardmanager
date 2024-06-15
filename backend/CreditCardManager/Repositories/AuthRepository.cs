using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CreditCardManager.Repositories
{
    /// <summary>
    /// Data repository for authorization
    /// </summary>
    public class AuthRepository : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public AuthRepository(DbContextOptions<AuthRepository> options) : base(options)
        {
        }
    }
}