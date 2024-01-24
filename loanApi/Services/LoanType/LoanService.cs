
using loanApi.Dtos;
using loanApi.Helper;
using loanApi.Models;
using loanApi.Services.LoanHistories;
using loanApi.Services.UserProfileService;
using System.Linq.Expressions;

namespace loanApi.Services.LoanType
{
    public class LoanService : ILoanService

    {
        public readonly IUserProfile _userProfile;
        private readonly ILoanHistoryRepository _loanHistoryRepository;
        private readonly ILoanTypeRepository _loanTypeRepository;
        private decimal OutstandingLoan {  get; set; }
        private int UserId { get; set; }

        public LoanService(IUserProfile userProfile, ILoanHistoryRepository loanHistoryRepository, ILoanTypeRepository loanTypeRepository)
        {
            _userProfile = userProfile;
            _loanHistoryRepository = loanHistoryRepository;
            _loanTypeRepository = loanTypeRepository;
        }
        public async Task<(bool, string)> CheckLoanEligibilityAsync(int userId)

        {
            UserId = userId; 
            var user = await _userProfile.GetUserProfile(userId);

            bool IsUserverified = user.ProfileUpdated;

            if (!IsUserverified)
            {
                return (false, "Not eligible. Please update your KYC.");

            }

            var userDebtProfile = await _loanHistoryRepository.GetLoanHistoryByUserId(userId);

            if (userDebtProfile == null)
            {
                OutstandingLoan = 0;
            }
            else
            {
                OutstandingLoan = userDebtProfile.Balance;
            }

             

            if (OutstandingLoan > 0)
            {
                return (false, $"Not eligible. You have an outstanding balance of {OutstandingLoan}");
            }

         
            return (true, "User eligible");
            
   
            }


        public async Task<int> DisburseLoan(LoanApplicationDto loanDetails)
        {
            LoanTypes loanType = await _loanTypeRepository.GetLoanTypeDetailsByIdAsync(loanDetails.loanTypeId);

            decimal interest = LoanCalculator.CalculateInterest(loanDetails.Amount, loanType.InterestRate, int.Parse(loanType.Duration));

            LoanHistory loan = new LoanHistory()
            {
                LoanAmount = loanDetails.Amount,
                Balance = loanDetails.Amount + interest,
                PaymentStatus = "Unpaid",
                Interest = interest,
                UserId = UserId,
                LoanPackageId = loanDetails.loanTypeId,
                Disbursed = true
            };


            var  (success, loanId) = await _loanHistoryRepository.AddLoanHistory(loan);

            return loanId;
        }

        public async Task<string> LoanDisbursementStatus(int loanId)


        {
            string message = "Your loan has been disbursed";

            LoanHistory loanHistory = await _loanHistoryRepository.GetLoanHistoryById(loanId);

            var status = loanHistory.Disbursed;

            if (loanHistory == null)
            {
                 message = "Loan does not exist";

            }   

            if (!status)
            {
                 message = "Not disbursed. Still under review, please check back. ";
            }

            return message; 

        }
    }
}
