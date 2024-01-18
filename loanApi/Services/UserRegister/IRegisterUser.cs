using loanApi.Models;
using loanApi.Services.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace loanApi.Services.UserRegister
{
    public interface IRegisterUser
    {
        Task<RegistrationResult> RegisterUserAsync(RegisterUsers registerUser);
        string ErrorMessage { get; }
    }
}
