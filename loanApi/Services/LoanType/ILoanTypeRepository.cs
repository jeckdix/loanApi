using loanApi.Models;

namespace loanApi.Services.LoanType
{
    public interface ILoanTypeRepository
    {
        Task<int> AddLoanAsync(LoanTypes loanTypes);
        Task<LoanTypes> GetStudentLoanDetailsAsync();
        Task<LoanTypes> GetEmergencyLoanDetailsAsync();
        Task<LoanTypes> GetBusinessLoanDetailsAsync();
        Task<LoanTypes> GetMortgageLoanDetailsAsync();
        Task<LoanTypes> GetLoanTypeDetailsByIdAsync(int loanTypeId);
    }
}
