
using loanApi.Data;
using Microsoft.EntityFrameworkCore;

namespace loanApi.Services.User
{
    public class UserRepo : IUser
    {
        private readonly DataContext _context;
        public UserRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<int?> GetUserByEmail(string email)
        {
           var user = await _context.userRegister.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }
    }
}
