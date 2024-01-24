using loanApi.Dtos;
using loanApi.Helper;
using loanApi.Models;
using loanApi.Services.LoanHistories;
using loanApi.Services.LoanPayments;
using loanApi.Services.LoanType;
using loanApi.Services.UserProfileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace loanApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoanController : ControllerBase
    {
        private readonly ILoanTypeRepository _loanTypeRepository;
        private readonly ILoanHistoryRepository _loanHistoryRepository;
        private readonly IUserProfile _userProfile;
        private readonly ILoanService _loanService;
        private readonly IPaymentRepository _paymentRepository;

        public LoanController(ILoanTypeRepository loanTypeRepository, ILoanHistoryRepository loanHistoryRepository, IUserProfile userProfile, ILoanService loanService = null, IPaymentRepository paymentRepository = null)
        {
            _loanTypeRepository = loanTypeRepository;
            _loanHistoryRepository = loanHistoryRepository;
            _userProfile = userProfile;

            _loanService = loanService;
            _paymentRepository = paymentRepository;
        }

        [HttpPost] 
        [Route("add")]
        public async Task<IActionResult> AddLoanAsync([FromBody] LoanTypes loanTypes)
        {
            try
            {
                int loanId = await _loanTypeRepository.AddLoanAsync(loanTypes);
                return Ok(new { LoanId = loanId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }     

        [HttpPost("request")]

        public async Task<IActionResult> LoanRequest( LoanApplicationDto loanRequestDetails)
        {
            string jwtToken = HttpContext.Request.Headers["Authorization"];
            int userId = Claims.GetCurrentUser(jwtToken);
            

            var   (isSuccess, message)  = await _loanService.CheckLoanEligibilityAsync(userId);

            if (!isSuccess)
            {
                return Ok(message);
            }

           int loanId =  await _loanService.DisburseLoan(loanRequestDetails); 
          

            return Ok($"Loan Request submitted with loan ID : {loanId}.");
        }

        [HttpGet("status/{loanId}")]

        public async Task <IActionResult> CheckLoanDisbursementStatus (int loanId)
        {

            var loanStatus = await _loanService.LoanDisbursementStatus(loanId);    
            
            return Ok(loanStatus); 

        }

        [HttpPost("payment")]

        public async Task <IActionResult> PayLoan (PaymentDto paymentDetails)
        {
            string jwtToken = HttpContext.Request.Headers["Authorization"];
            int userId = Claims.GetCurrentUser(jwtToken);

            if (paymentDetails == null )
            {
                return BadRequest();
            }

            Payment payment = new Payment()
            {
                Amount = paymentDetails.Amount,
                LoanId = paymentDetails.loanId,
                UserId = userId,
            };

            var paymentId = await _paymentRepository.MakePaymentAsync(payment); 

            return Ok($"{paymentDetails.Amount} deducted successfully. Payment reference Id: {paymentId}");
        }
        
    }

}
