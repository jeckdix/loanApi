using loanApi.Models;

namespace loanApi.Services.LoanHistories
{
    public interface ILoanHistoryRepository
    {
        Task<ICollection<LoanHistory>> GetLoanHistories();
        Task<LoanHistory> GetLoanHistoryById(int id);
        Task<(bool, int)> AddLoanHistory(LoanHistory loanHistory);
        Task<bool> UpdateLoanHistory(LoanHistory loanHistory);
        Task<bool> LoanHistoryExists(int userId);
        Task<IEnumerable<LoanHistory>> GetLoanHistoriesByUserId(int userId);
        Task<LoanHistory> GetLoanHistoryByUserId(int userId);
        //Task<bool> CreateLoanHistoryForUser(int userId, LoanPackages selectedLoanPackage);
        //Task<bool> UpdateLoanHistoryFromPayment(int loanHistoryId, Payment payment);
        Task<bool> SaveAsync();
    }
}
