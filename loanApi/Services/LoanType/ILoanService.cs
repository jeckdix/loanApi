using loanApi.Dtos;

namespace loanApi.Services.LoanType
{
    public interface ILoanService
    {
        Task<(bool, string)> CheckLoanEligibilityAsync(int userId);
        Task<int> DisburseLoan(LoanApplicationDto loanDetails);

        Task<string> LoanDisbursementStatus(int loanId);
    }
}
