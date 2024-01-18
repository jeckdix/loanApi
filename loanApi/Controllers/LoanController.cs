using loanApi.Dtos;
using loanApi.Helper;
using loanApi.Models;
using loanApi.Services.LoanType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace loanApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanTypeRepository _loanTypeRepository;

        public LoanController(ILoanTypeRepository loanTypeRepository)
        {
            _loanTypeRepository = loanTypeRepository;
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
                LoanTypes studentLoan = await  _loanTypeRepository.GetStudentLoanDetailsAsync();

                if(studentLoan == null)
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
                return Ok(new { InterestAmount = interest });
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
                return Ok(new {
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
                // Assuming _loanTypeRepository has a method to get loan details by ID
                LoanTypes loanType = await _loanTypeRepository.GetLoanTypeDetailsByIdAsync(loanTypeId);

                if (loanType == null)
                {
                    return NotFound("No Loan Of Such Found");
                }

                decimal interest = LoanCalculator.CalculateInterest(loanType.MaxLoanAmount, loanType.InterestRate, int.Parse(loanType.Duration));
                return Ok(new { InterestAmount = interest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
