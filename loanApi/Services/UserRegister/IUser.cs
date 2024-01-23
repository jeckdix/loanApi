using loanApi.Models;
using loanApi.Services.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace loanApi.Services.UserRegister
{
    public interface IUser
    {
        Task<RegistrationResult> RegisterUserAsync(Models.User registerUser);
        string ErrorMessage { get; }
    }
}
