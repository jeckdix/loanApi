
using loanApi.Data;
using loanApi.Models;
using loanApi.Services.UserRegister;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace loanApi.Services.RegisterUser
{
    public class RegisterService : IRegisterUser
    {
        private readonly DataContext _dataContext;
        private readonly Random _random;
        private readonly ILogger<RegisterService> _logger;

        public RegisterService(DataContext dataContext, ILogger<RegisterService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
            _random = new Random();
        }


        public string ErrorMessage { get; private set; }
        public async Task<RegistrationResult> RegisterUserAsync(RegisterUsers registerUser)
        {
            try
            {
                if (await _dataContext.userRegister.AnyAsync(u => u.Email == registerUser.Email))
                {
                    return RegistrationResult.EmailAlreadyExists;
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
                registerUser.Password = hashedPassword;

                var newUser = new RegisterUsers
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    OTP = GenerateOTP(),
                    Email = registerUser.Email,
                    Password = registerUser.Password
                };

                await SendOtpEmail(newUser.Email, newUser.OTP);

                _dataContext.userRegister.Add(newUser);
                await _dataContext.SaveChangesAsync();

              

                _logger.LogInformation($"User registered: {newUser.Email}");

                return RegistrationResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                return RegistrationResult.Failure;
            }
        }

        private string GenerateOTP()
        {
            string otp = _random.Next(100000, 999999).ToString();
            return otp;
        }

        private async Task SendOtpEmail(string email, string otp)
        {
            try
            {
                using MailMessage mail = new MailMessage();
                {
                    mail.From = new MailAddress("lukemorolakemi@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "OTP REGISTRATION CODE";
                    mail.Body = $"Your OTP for registration is: {otp}";

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("lukemorolakemi@gmail.com", "qcyz ypqk pslv mlmt");
                        smtp.EnableSsl = true;

                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send OTP: {ErrorMessage}", ex.Message);
                throw new Exception($"Failed to send OTP: {ex.Message}");
            }
        }
    }

    public enum RegistrationResult
    {
        Success,
        EmailAlreadyExists,
        Failure
    }
}

