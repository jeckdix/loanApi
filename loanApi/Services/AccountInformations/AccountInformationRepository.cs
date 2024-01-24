using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loanApi.Data;
using loanApi.Models;
using Microsoft.EntityFrameworkCore;

namespace loanApi.Services.AccountInformations
{
    public class AccountInformationRepository : IAccountInformationRepository
    {
        private readonly DataContext _dbContext;

        public AccountInformationRepository(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> AddAccountInformation(AccountInformation accountInformation)
        {
            _dbContext.accountInformations.Add(accountInformation);
            return await Save();
        }

        public async Task<bool> AccountExists(int id)
        {
            return await _dbContext.accountInformations.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> DeleteAccountInformation(int id)
        {
            var accountInformation = await _dbContext.accountInformations.FindAsync(id);

            if (accountInformation == null)
                return false;

            _dbContext.accountInformations.Remove(accountInformation);
            return await Save();
        }

        public async Task<AccountInformation> GetAccountInformationById(int id)
        {
            return await _dbContext.accountInformations.FindAsync(id);
        }

        public async Task<ICollection<AccountInformation>> GetAccounts()
        {
            return await _dbContext.accountInformations.ToListAsync();
        }

        public async Task<bool> Save()
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

        public async Task<bool> UpdateAccountInformation(AccountInformation accountInformation)
        {
            _dbContext.accountInformations.Update(accountInformation);
            return await Save();
        }
    }
}
