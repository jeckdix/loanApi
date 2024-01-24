using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loanApi.Data;
using loanApi.Models;
using Microsoft.EntityFrameworkCore;

namespace loanApi.Services.CardDetails
{
    public class CardDetailsRepository : ICardDetailsRepository
    {
        private readonly DataContext _dbContext;

        public CardDetailsRepository(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ICollection<CardDetail>> GetCards()
        {
            return await _dbContext.cardDetails.ToListAsync();
        }

        public async Task<CardDetail> GetCardDetailsById(int id)
        {
            return await _dbContext.cardDetails.FindAsync(id);
        }

        public async Task<bool> AddCardDetails(CardDetail cardDetails)
        {
            _dbContext.cardDetails.Add(cardDetails);
            return await SaveAsync();
        }

        public async Task<bool> UpdateCardDetails(CardDetail cardDetails)
        {
            _dbContext.cardDetails.Update(cardDetails);
            return await SaveAsync();
        }

        public async Task<bool> DeleteCardDetails(int id)
        {
            var cardDetails = await _dbContext.cardDetails.FindAsync(id);

            if (cardDetails == null)
                return false;

            _dbContext.cardDetails.Remove(cardDetails);
            return await SaveAsync();
        }

        public async Task<bool> CardExists(int adminid)
        {
            return await _dbContext.cardDetails.AnyAsync(c => c.Id == adminid);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dbContext.SaveChangesAsync();
            return saved > 0;
            //try
            //{
            //    return await _dbContext.SaveChangesAsync() > 0;
            //}
            //catch (DbUpdateException)
            //{
            //    // Handle DbUpdateException as needed
            //    return false;
            //}
        }
    }
}
