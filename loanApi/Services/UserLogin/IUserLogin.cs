using loanApi.Dtos;

namespace loanApi.Services.UserLogin
{
    public interface IUserLogin
    {
        Task<string> AuthenticateUserAsync(LoginDto loginUser);
    }
}
