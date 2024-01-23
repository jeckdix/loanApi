using loanApi.Data;
using loanApi.Models;
using Microsoft.EntityFrameworkCore;

namespace loanApi.Services.LoanType
{
    public class LoanTypeRepository : ILoanTypeRepository
    {
        private readonly DataContext _dataContext;

        public LoanTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> AddLoanAsync(LoanTypes loanTypes)
        {
           _dataContext.Loantypes.Add(loanTypes);
            await _dataContext.SaveChangesAsync();
            return loanTypes.Id;
        }

        public async Task<LoanTypes> GetBusinessLoanDetailsAsync()
        {
           return await _dataContext.Loantypes.FirstOrDefaultAsync(loan => loan.LoanName == "Business Loan");
        }

        public async Task<LoanTypes> GetEmergencyLoanDetailsAsync()
        {
            return await _dataContext.Loantypes.FirstOrDefaultAsync(loan => loan.LoanName == "Emergency Loan");
        }

        public async Task<LoanTypes> GetLoanTypeDetailsByIdAsync(int loanTypeId)
        {
            return await _dataContext.Loantypes.FindAsync(loanTypeId);
        }

        public async Task<LoanTypes> GetMortgageLoanDetailsAsync()
        {
            return await _dataContext.Loantypes.FirstOrDefaultAsync(loan => loan.LoanName == "Mortgage Loan");
        }

        public async Task<LoanTypes> GetStudentLoanDetailsAsync()
        {
          
            return await _dataContext.Loantypes.FirstOrDefaultAsync(loan => loan.LoanName == "Student Loan ");
          
        }

   }

    
}
