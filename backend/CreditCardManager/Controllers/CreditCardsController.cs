using CreditCardManager.Models;
using CreditCardManager.Repositories;
using CreditCardManager.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CreditCardManager.Controllers
{
    /// <summary>
    /// API Controller to register and view credit cards
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CreditCardsController> _logger;
        private readonly CreditCardRepository _cardRepository;
        private readonly DataProtectionManager dataProtectionManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="logger"></param>
        /// <param name="creditCardRepository"></param>
        /// <param name="dataProtectionManager"></param>
        public CreditCardsController(IMemoryCache memoryCache,
                                     ILogger<CreditCardsController> logger,
                                     CreditCardRepository creditCardRepository,
                                     DataProtectionManager dataProtectionManager)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _cardRepository = creditCardRepository;
            this.dataProtectionManager = dataProtectionManager;
        }

        // GET: api/CreditCards
        /// <summary>
        /// Gets all registered credit cards in paginated mode.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<RegisteredCreditCard>>> GetCreditCards(int page = 1, int size = 10)
        {
            var cards = await _cardRepository.RegisteredCards.OrderBy(x => x.ExpiryDate)
                                                  .Skip((page - 1) * size)
                                                  .Take(size)
                                                  .ToListAsync();

            return cards;
        }

        // GET: api/CreditCards/id
        /// <summary>
        /// Gets registered credit card by id.
        /// </summary>
        /// <param name="id">Credit card identifier</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        public async Task<ActionResult<RegisteredCreditCard>> GetCreditCard(Guid id)
        {
            if (_memoryCache.TryGetValue(id, out RegisteredCreditCard? cachedCard))
                return cachedCard;
            else
            {
                var creditCard = await _cardRepository.RegisteredCards.FindAsync(id);
                if (creditCard == null)
                {
                    return NotFound();
                }
                _memoryCache.Set(id, creditCard);
                return creditCard;
            }
        }

        // POST: api/CreditCards
        /// <summary>
        /// Registers new credit card in the system.
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<RegisteredCreditCard>> PostCreditCard(CreditCard creditCard)
        {
            var existingCard = await _cardRepository.RegisteredCards.FindAsync(creditCard.Id);
            if (existingCard == null)
            {
                RegisteredCreditCard secureCard = dataProtectionManager.Protect(creditCard);
                _cardRepository.RegisteredCards.Add(secureCard);
                await _cardRepository.SaveChangesAsync();
                _memoryCache.Set(secureCard.Id, secureCard);
                return CreatedAtAction("GetCreditCard", new { id = creditCard.Id }, secureCard);
            }
            else
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Global error handler
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            var ex = exceptionHandlerFeature.Error;

            _logger.LogError(exception: ex, message: ex.Message);

            return Problem();
        }
    }
}