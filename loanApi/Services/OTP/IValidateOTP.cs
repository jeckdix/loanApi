namespace loanApi.Services.OTP
{
    public interface IValidateOTP
    {
        Task<OTPValidationResult> ValidateOTPAsync(string otp);
    }
}
