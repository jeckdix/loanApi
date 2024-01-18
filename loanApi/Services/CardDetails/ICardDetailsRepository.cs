using System.Collections.Generic;
using System.Threading.Tasks;
using loanApi.Models;

namespace loanApi.Services.CardDetails
{
    public interface ICardDetailsRepository
    {
        Task<ICollection<CardDetail>> GetCards();
        Task<CardDetail> GetCardDetailsById(int id);
        Task<bool> AddCardDetails(CardDetail cardDetails);
        Task<bool> UpdateCardDetails(CardDetail cardDetails);
        Task<bool> DeleteCardDetails(int id);
        Task<bool> CardExists(int adminid);
        Task<bool> SaveAsync();
        // Add other asynchronous methods as needed
    }
}
