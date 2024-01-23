using loanApi.Dtos;
using loanApi.Helper;
using loanApi.Models;
using loanApi.Services.LoanHistories;
using loanApi.Services.LoanType;
using loanApi.Services.User;
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
        private readonly IUser _user;

        public LoanController(ILoanTypeRepository loanTypeRepository, ILoanHistoryRepository loanHistoryRepository, IUserProfile userProfile, IUser user = null)
        {
            _loanTypeRepository = loanTypeRepository;
            _loanHistoryRepository = loanHistoryRepository;
            _userProfile = userProfile;
            _user = user;
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

        [HttpGet("calculate-student-loan-interest")]
        public async Task<IActionResult> CalculateStudentLoanInterest()
        {
            try
            {
                LoanTypes studentLoan = await _loanTypeRepository.GetStudentLoanDetailsAsync();

                if (studentLoan == null)
                {
                    return NotFound("No Loan Of Such Found");
                }

                decimal interest = LoanCalculator.CalculateInterest(studentLoan.MaxLoanAmount, studentLoan.InterestRate, int.Parse(studentLoan.Duration));
                return Ok(new { InterestAmount = interest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("calculate-emergency-loan-interest")]
        public async Task<IActionResult> CalculateEmergencyLoanInterest()
        {
            try
            {
                LoanTypes emergencyLoan = await _loanTypeRepository.GetEmergencyLoanDetailsAsync();

                if (emergencyLoan == null)
                {
                    return NotFound("No Loan Of Such Found");
                }

                decimal interest = LoanCalculator.CalculateInterest(emergencyLoan.MaxLoanAmount, emergencyLoan.InterestRate, int.Parse(emergencyLoan.Duration));
                return Ok(new { InterestAmount = interest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("calculate-mortgage-loan-interest")]
        public async Task<IActionResult> CalculateMortgageLoanInterest()
        {
            try
            {
                LoanTypes mortgageLoan = await _loanTypeRepository.GetMortgageLoanDetailsAsync();

                if (mortgageLoan == null)
                {
                    return NotFound("No Loan Of Such Found");
                }

                decimal interest = LoanCalculator.CalculateInterest(mortgageLoan.MaxLoanAmount, mortgageLoan.InterestRate, int.Parse(mortgageLoan.Duration));
                return Ok(new
                {
                    InterestAmount = interest
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost("calculate-mortgage-loan-interest")]
        //public async Task<IActionResult> CalculateMortgageLoanInterest([FromBody] LoanCalculationRequest loanRequest)
        //{
        //    try
        //    {
        //        LoanTypes mortgageLoan = await _loanTypeRepository.GetMortgageLoanDetailsAsync();

        //        if (mortgageLoan == null)
        //        {
        //            return NotFound("No Loan Of Such Found");
        //        }

        //        // Validate and parse the user-entered loan amount
        //        if (!decimal.TryParse(loanRequest.UserEnteredLoanAmount, out decimal userEnteredLoanAmount))
        //        {
        //            return BadRequest("Invalid loan amount. Please enter a valid numeric value.");
        //        }

        //        // Calculate interest using the user-entered loan amount
        //        decimal interest = LoanCalculator.CalculateInterest(userEnteredLoanAmount, mortgageLoan.InterestRate, int.Parse(mortgageLoan.Duration));
        //        return Ok(new { InterestAmount = interest });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        [HttpGet("calculate-business-loan-interest")]
        public async Task<IActionResult> CalculateBusinessLoanInterest()
        {
            try
            {
                LoanTypes businessLoan = await _loanTypeRepository.GetBusinessLoanDetailsAsync();

                if (businessLoan == null)
                {
                    return NotFound("No Loan Of Such Found");
                }

                decimal interest = LoanCalculator.CalculateInterest(businessLoan.MaxLoanAmount, businessLoan.InterestRate, int.Parse(businessLoan.Duration));
                return Ok(new
                {
                    InterestAmount = interest,
                    InterestRate = businessLoan.InterestRate,
                    MaxLoanDuration = businessLoan.Duration,
                    MinLoanAmount = businessLoan.MinLoanAmount,
                    MaxLoanAmount = businessLoan.MaxLoanAmount,
                    LoanName = businessLoan.LoanName

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("calculate-loan-interest/{loanTypeId}")]
        public async Task<IActionResult> CalculateLoanInterest(int loanTypeId)
        {
            try
            {

                LoanTypes loanType = await _loanTypeRepository.GetLoanTypeDetailsByIdAsync(loanTypeId);

                if (loanType == null)
                {
                    return NotFound("No Loan Of Such Found");
                }

                decimal interest = LoanCalculator.CalculateInterest(loanType.MaxLoanAmount, loanType.InterestRate, int.Parse(loanType.Duration));
                return Ok(new
                {
                    LoanTypeId = loanType.Id,
                    InterestAmount = interest,
                    MaxLoanAmount = loanType.MaxLoanAmount,
                    MinLoanAmount = loanType.MinLoanAmount,
                    InterestRate = loanType.InterestRate,
                    Duration = loanType.Duration,

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("get-loan")]

        public async Task<ActionResult> GetLoan([FromBody] LoanApplicationDto loanDetails)
        {
            string jwtToken = HttpContext.Request.Headers["Authorization"];
         
            //var jt = User.Claims;

            var th = new JwtSecurityTokenHandler();

            var rdth = th.ReadToken(jwtToken.Replace("Bearer ", "")) as JwtSecurityToken;

            IEnumerable<Claim> claims = rdth.Claims;


            var c = JsonConvert.SerializeObject(claims, Formatting.Indented);

            var myclaims = claims.Where(x => x.Type == ClaimTypes.Role || x.Type == ClaimTypes.NameIdentifier);

            var userId = int.Parse(claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());

           
            
            var user = await _userProfile.GetUserProfile(userId);


            bool verified = user.ProfileUpdated;

            var userDebtProfile = await _loanHistoryRepository.GetLoanHistoryByUserId(userId);

            decimal debtOutstanding = 0; 

            if ( userDebtProfile != null )
            {
                 debtOutstanding = userDebtProfile.Balance;
            }



            if (!verified)
            {
                return Ok("Account KYC  not updated");
            }
            
            if (debtOutstanding > 1000000)
            {
                return Ok($"Sorry, you are not eligible to borrow. You have an outstanding balance of {-debtOutstanding}");
            }



            if (debtOutstanding < 1000000)
            {

                LoanTypes loanType = await _loanTypeRepository.GetLoanTypeDetailsByIdAsync(loanDetails.loanTypeId);


                if (loanType != null)
                {
                    if (loanDetails.Amount > loanType.MaxLoanAmount)
                    {
                        return Ok($"The maximum amount you can borrow is {loanType.MaxLoanAmount}");
                    }
                    
                    
                    decimal interest = LoanCalculator.CalculateInterest(loanDetails.Amount, loanType.InterestRate, int.Parse(loanType.Duration));

                    LoanHistory loan = new LoanHistory()
                    {
                        LoanAmount = loanDetails.Amount,
                        Balance = debtOutstanding + loanDetails.Amount + interest,
                        Status = "Unpaid",
                        Interest = interest,
                        UserId = userId,
                        LoanPackageId = loanDetails.loanTypeId
                    };

                    await _loanHistoryRepository.AddLoanHistory(loan);

                    return Ok($"{loanDetails.Amount} disbursed successfully");

                }


                return Ok();

            }

            return BadRequest();
        }
    }

}
