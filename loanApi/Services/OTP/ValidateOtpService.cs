using loanApi.Data;
using Microsoft.EntityFrameworkCore;

namespace loanApi.Services.OTP
{
    public class ValidateOtpService : IValidateOTP
    {
        private readonly DataContext _dataContext;

        public ValidateOtpService(DataContext dataContext)
        {
            _dataContext = dataContext;          
        }

        public async Task<OTPValidationResult> ValidateOTPAsync(string otp)
        {
            try
            {
                // Find the corresponding entry in the database using the provided OTP
                var user = await _dataContext.userRegister.FirstOrDefaultAsync(a => a.OTP == otp);

                if (user != null)
                {
                    // Valid OTP
                    return OTPValidationResult.Success;
                }
                else
                {
                    // Invalid OTP or admin not found
                    return OTPValidationResult.InvalidOTP;
                }
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

