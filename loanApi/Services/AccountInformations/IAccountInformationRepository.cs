using System.Collections.Generic;
using System.Threading.Tasks;
using loanApi.Models;

namespace loanApi.Services.AccountInformations
{
    public interface IAccountInformationRepository
    {
        Task<bool> AddAccountInformation(AccountInformation accountInformation);
        Task<bool> AccountExists(int id);
        Task<bool> DeleteAccountInformation(int id);
        Task<AccountInformation> GetAccountInformationById(int id);
        Task<ICollection<AccountInformation>> GetAccounts();
        Task<bool> Save();
        Task<bool> UpdateAccountInformation(AccountInformation accountInformation);
    }
}
