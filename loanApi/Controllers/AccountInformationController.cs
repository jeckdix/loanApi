using Microsoft.AspNetCore.Mvc;
using loanApi.Models;
using loanApi.Services.AccountInformations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Import the namespace for Task

namespace loanApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountInformationController : ControllerBase
    {
        private readonly IAccountInformationRepository _accountInformationRepository;

        public AccountInformationController(IAccountInformationRepository accountInformationRepository)
        {
            _accountInformationRepository = accountInformationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _accountInformationRepository.GetAccounts();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _accountInformationRepository.GetAccountInformationById(id);

            if (account == null)
                return NotFound();

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> AddAccountInformation([FromBody] AccountInformation accountInformation)
        {
            if (accountInformation == null)
                return BadRequest();

            await _accountInformationRepository.AddAccountInformation(accountInformation);

            return CreatedAtAction(nameof(GetAccountById), new { id = accountInformation.Id }, accountInformation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountInformation(int id, [FromBody] AccountInformation accountInformation)
        {
            if (accountInformation == null || id != accountInformation.Id)
                return BadRequest();

            if (!await _accountInformationRepository.AccountExists(id))
                return NotFound();

            await _accountInformationRepository.UpdateAccountInformation(accountInformation);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountInformation(int id)
        {
            if (!await _accountInformationRepository.AccountExists(id))
                return NotFound();

            await _accountInformationRepository.DeleteAccountInformation(id);

            return NoContent();
        }
    }
}
