using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;  // Ensure you have this namespace for Entity Framework
using loanApi.Data;
using loanApi.Models;
using loanApi.Services.LoanHistories;

namespace loanApi.Services.LoanHistories
{
    public class LoanHistoryRepository : ILoanHistoryRepository
    {
        private readonly DataContext _dataContext;

        public LoanHistoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<LoanHistory>> GetLoanHistories()
        {
            return await _dataContext.loanHistories.ToListAsync();
        }

        public async Task<LoanHistory> GetLoanHistoryById(int id)
        {
            return await _dataContext.loanHistories.FirstOrDefaultAsync(loanHistory => loanHistory.Id == id);
        }

        public async Task<LoanHistory> GetLoanHistoryByUserId(int userId)
        {
            return await _dataContext.loanHistories.Where(loanHistory => loanHistory.UserId == userId)
                    .OrderByDescending(loanHistory => loanHistory.Date).FirstOrDefaultAsync();


        }

        public async Task<(bool, int)> AddLoanHistory(LoanHistory loanHistory)
        {
            _dataContext.loanHistories.Add(loanHistory);
             await SaveAsync();

            return (true, loanHistory.Id); 
        }

        public async Task<bool> UpdateLoanHistory(LoanHistory loanHistory)
        {
            var existingLoanHistory = await _dataContext.loanHistories.FirstOrDefaultAsync(lh => lh.Id == loanHistory.Id);
            if (existingLoanHistory != null)
            {
                // Update relevant properties
                existingLoanHistory.Balance = loanHistory.Balance;
                existingLoanHistory.PaymentStatus = loanHistory.PaymentStatus;
                // Update other properties as needed

                return await SaveAsync();
            }

            return false; // Loan history not found
        }

        public async Task<bool> LoanHistoryExists(int userId)
        {
            return await _dataContext.loanHistories.AnyAsync(loanHistory => loanHistory.UserId == userId);
        }

        public async Task<IEnumerable<LoanHistory>> GetLoanHistoriesByUserId(int userId)
        {
            var userLoanHistories = await _dataContext.loanHistories.Where(loanHistory => loanHistory.UserId == userId).ToListAsync();
            return userLoanHistories;
        }

        public async Task<bool> SaveAsync()
        {
            // Perform any necessary persistence logic here (e.g., saving changes to the database)
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
