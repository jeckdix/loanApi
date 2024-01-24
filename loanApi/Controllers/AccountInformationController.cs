using Microsoft.AspNetCore.Mvc;
using loanApi.Models;
using loanApi.Services.AccountInformations;
using System.Threading.Tasks;
using AutoMapper;
using loanApi.Dtos;
using Microsoft.AspNetCore.Authorization; // Import the namespace for Task

namespace loanApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountInformationController : ControllerBase
    {
        private readonly IAccountInformationRepository _accountInformationRepository;
        private readonly IMapper _mapper;

        public AccountInformationController(IAccountInformationRepository accountInformationRepository, IMapper mapper)
        {
            _accountInformationRepository = accountInformationRepository;
            _mapper = mapper;
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
        public async Task<IActionResult> AddAccountInformation([FromBody] AccountDto accountInformation)
        {
            if (accountInformation == null)
                return BadRequest();

            var accountMap = _mapper.Map<AccountInformation>(accountInformation);
            await _accountInformationRepository.AddAccountInformation(accountMap);

            return CreatedAtAction(nameof(GetAccountById), new { id = accountInformation.Id }, accountInformation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountInformation(int id, [FromBody] AccountDto accountInformation)
        {
            if (accountInformation == null || id != accountInformation.Id)
                return BadRequest();

            if (!await _accountInformationRepository.AccountExists(id))
                return NotFound();

            var accountMap = _mapper.Map<AccountInformation>(accountInformation);
            await _accountInformationRepository.UpdateAccountInformation(accountMap);

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
