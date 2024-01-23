using AutoMapper;
using loanApi.Dtos;
using loanApi.Models;
using loanApi.Services.OTP;
using loanApi.Services.RegisterUser;
using loanApi.Services.UserLogin;
using loanApi.Services.UserRegister;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace loanApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUser _registerUserService;
        private readonly IUserLogin _userLoginService;
        private readonly IValidateOTP _validateOTP;
        private readonly IMapper _mapper;

        public UserController(IUser registerUser, IUserLogin userLogin, IValidateOTP validateOTP, IMapper mapper)
        {
            _registerUserService = registerUser;
            _userLoginService = userLogin;
            _validateOTP = validateOTP;
            _mapper = mapper;
        }

   
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input data");
            }
            var registerMap = _mapper.Map<User>(registerUser);
            var registrationResult = await _registerUserService.RegisterUserAsync(registerMap);

            switch (registrationResult)
            {
                case RegistrationResult.Success:
                    return Ok("User registered successfully");
                case RegistrationResult.EmailAlreadyExists:
                    return BadRequest("Email already exists");
                case RegistrationResult.Failure:
                default:
                    return StatusCode(500, $"Registration failed: {registrationResult}");
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            // Check if the provided DTO is valid (e.g., required fields are not null)
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            // Authenticate the user using the injected service
            string token = await _userLoginService.AuthenticateUserAsync(login);

            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }

            // If credentials are valid, return the JWT token
            return Ok(new { Token = token });
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateOTP(ValidateOtpDto validateOTP)
        {
            if (validateOTP == null)
            {
                return BadRequest("Invalid OTP data");
            }

            // Use the injected service to validate the OTP
            var result = await _validateOTP.ValidateOTPAsync(validateOTP.OTP);

            switch (result)
            {
                case OTPValidationResult.Success:
                    return Ok("OTP validated successfully!");
                case OTPValidationResult.InvalidOTP:
                    return BadRequest("Invalid OTP or admin not found");
                case OTPValidationResult.Failure:
                default:
                    return StatusCode(500, "An error occurred while validating OTP");
            }
        }


    }
}
