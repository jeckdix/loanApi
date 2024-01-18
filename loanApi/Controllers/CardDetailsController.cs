using Microsoft.AspNetCore.Mvc;
using loanApi.Models;
using loanApi.Services.CardDetails;
using System.Threading.Tasks;

namespace loanApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardDetailsController : ControllerBase
    {
        private readonly ICardDetailsRepository _cardDetailsRepository;

        public CardDetailsController(ICardDetailsRepository cardDetailsRepository)
        {
            _cardDetailsRepository = cardDetailsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<CardDetail>>> GetCards()
        {
            var cards = await _cardDetailsRepository.GetCards();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardDetail>> GetCardById(int id)
        {
            var card = await _cardDetailsRepository.GetCardDetailsById(id);

            if (card == null)
                return NotFound();

            return Ok(card);
        }

        [HttpPost]
        public async Task<ActionResult<CardDetail>> AddCardDetails([FromBody] CardDetail cardDetails)
        {
            if (cardDetails == null)
                return BadRequest();

            await _cardDetailsRepository.AddCardDetails(cardDetails);

            return CreatedAtAction(nameof(GetCardById), new { id = cardDetails.Id }, cardDetails);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCardDetails(int id, [FromBody] CardDetail cardDetails)
        {
            if (cardDetails == null || id != cardDetails.Id)
                return BadRequest();

            if (!await _cardDetailsRepository.CardExists(id))
                return NotFound();

            await _cardDetailsRepository.UpdateCardDetails(cardDetails);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardDetails(int id)
        {
            if (!await _cardDetailsRepository.CardExists(id))
                return NotFound();

            await _cardDetailsRepository.DeleteCardDetails(id);

            return NoContent();
        }
    }
}
