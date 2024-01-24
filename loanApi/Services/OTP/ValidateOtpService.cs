using loanApi.Data;
using loanApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace loanApi.Services.OTP
{
    public class ValidateOtpService : IValidateOTP
    {
        private readonly DataContext _dataContext;
        private readonly IMemoryCache _cache;

        public ValidateOtpService(DataContext dataContext, IMemoryCache cache)
        {
            _dataContext = dataContext;
            _cache = cache;
        }

        public async Task<OTPValidationResult> ValidateOTPAsync(string otp)
        {
            try
            {
                // Find the corresponding entry in the database using the provided OTP
                //var user = await _dataContext.userRegister.FirstOrDefaultAsync(a => a.OTP == otp);

                var userExists = _cache.TryGetValue("tempUser", out User newUser);

                if (userExists)
                {
                    if (newUser.OTP == otp)
                    {
                        //create user from cache and save to db

                        _dataContext.Users.Add(newUser);
                        await _dataContext.SaveChangesAsync();

                        // Valid OTP
                        return OTPValidationResult.Success;
                    }
                    else
                    {
                        // Invalid OTP or admin not found
                        return OTPValidationResult.InvalidOTP;
                    }
                }
                else
                    throw new Exception("Something wrong happened when retrieving OTP");
                
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return OTPValidationResult.Failure;
            }
        }

    }

    public enum OTPValidationResult
    {
        Success,
        InvalidOTP,
        Failure
    }
}

