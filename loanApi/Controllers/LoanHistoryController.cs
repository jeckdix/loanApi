using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using loanApi.Models;
using loanApi.Services.LoanHistories;
using Microsoft.AspNetCore.Authorization;

namespace loanApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LoanHistoryController : ControllerBase
    {
        private readonly ILoanHistoryRepository _loanHistoryRepository;

        public LoanHistoryController(ILoanHistoryRepository loanHistoryRepository)
        {
            _loanHistoryRepository = loanHistoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanHistory>>> GetLoanHistories()
        {
            var loanHistories = await _loanHistoryRepository.GetLoanHistories();
            return Ok(loanHistories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanHistory>> GetLoanHistoryById(int id)
        {
            var loanHistory = await _loanHistoryRepository.GetLoanHistoryById(id);

            if (loanHistory == null)
            {
                return NotFound();
            }

            return Ok(loanHistory);
        }

        //[HttpPost]
        //public async Task<ActionResult> AddLoanHistory([FromBody] LoanHistory loanHistory)
        //{
        //    var (success, loanId)  = await _loanHistoryRepository.AddLoanHistory(loanHistory);

        //    if (success)
        //    {
        //        return CreatedAtAction(nameof(GetLoanHistoryById), new { id = loanHistory.Id }, loanHistory);
        //    }

        //    return BadRequest("Failed to add loan history");
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> UpdateLoanHistory(int id, [FromBody] LoanHistory loanHistory)
        //{
        //    if (id != loanHistory.Id)
        //    {
        //        return BadRequest("Mismatched IDs");
        //    }

        //    var success = await _loanHistoryRepository.UpdateLoanHistory(loanHistory);

        //    if (success)
        //    {
        //        return NoContent();
        //    }

        //    return NotFound("Loan history not found");
        //}

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<LoanHistory>>> GetLoanHistoriesByUserId(int userId)
        {
            var loanHistories = await _loanHistoryRepository.GetLoanHistoriesByUserId(userId);
            return Ok(loanHistories);
        }

        // Add other actions as needed

        // Example:
        // [HttpPost("createForUser/{userId}")]
        // public async Task<ActionResult> CreateLoanHistoryForUser(int userId, [FromBody] LoanPackages selectedLoanPackage)
        // {
        //     var success = await _loanHistoryRepository.CreateLoanHistoryForUser(userId, selectedLoanPackage);
        // 
        //     if (success)
        //     {
        //         return Ok("Loan history created for user");
        //     }
        // 
        //     return BadRequest("Failed to create loan history");
        // }
    }
}
