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
using static loanApi.Helper.CustomLoginResponse;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static loanApi.Helper.CustomRegisterResponse;
using loanApi.Helper;

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
        private readonly ILogger<UserService> _logger;

        public UserController(IUser registerUser, IUserLogin userLogin, IValidateOTP validateOTP, IMapper mapper, ILogger<UserService> logger)
        {
            _registerUserService = registerUser;
            _userLoginService = userLogin;
            _validateOTP = validateOTP;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto registerUser)
        {
            try
            {
                var registerMap = _mapper.Map<User>(registerUser);
                var registrationResult = await _registerUserService.RegisterUserAsync(registerMap);

                // Custom response based on the registration result
                var response = registrationResult switch
                {
                    RegistrationResult.Success => new CustomNewResponse
                    {
                        Message = "User registered successfully",
                        Success = true,
                        UserDetails = new UserDetail
                        {
                            FirstName = registerUser.FirstName,
                            LastName = registerUser.LastName,
                            Email = registerUser.Email
                            // You may exclude Password from being returned in the response for security reasons.
                            // Password = registerUser.Password
                        }
                    },
                    RegistrationResult.EmailAlreadyExists => new CustomNewResponse { Message = "Email already exists", Success = false },
                    RegistrationResult.Failure => new CustomNewResponse { Message = "Registration failed", Success = false },
                    _ => new CustomNewResponse { Message = "Unknown registration result", Success = false }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new CustomNewResponse { Message = "Internal server error", Success = false });
            }
        }


        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginDto login)
        //{
        //    // Check if the provided DTO is valid (e.g., required fields are not null)
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid input");
        //    }

        //    // Authenticate the user using the injected service
        //    string token = await _userLoginService.AuthenticateUserAsync(login);

        //    if (token == null)
        //    {
        //        return Unauthorized("Invalid credentials");
        //    }

        //    // If credentials are valid, return the JWT token
        //    return Ok(new { Token = token });
        //}
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

            // Retrieve user details from the token and create a UserDetail object
            var userClaims = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var userDetail = new Detail
            {
                FirstName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                Email = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                // You may exclude Password from being returned in the response for security reasons.
                // Password = userClaims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Password)?.Value
            };

            // Create the custom response object
            var response = new LoginResponse
            {
                Token = token,
                UserDetails = userDetail
            };

            // If credentials are valid, return the JWT token and user details
            return Ok(response);
        }

        //[HttpPost("validate")]
        //public async Task<IActionResult> ValidateOTP(ValidateOtpDto validateOTP)
        //{
        //    if (validateOTP == null)
        //    {
        //        return BadRequest("Invalid OTP data");
        //    }

        //    // Use the injected service to validate the OTP
        //    var result = await _validateOTP.ValidateOTPAsync(validateOTP.OTP);

        //    switch (result)
        //    {
        //        case OTPValidationResult.Success:
        //            return Ok("OTP validated successfully!");
        //        case OTPValidationResult.InvalidOTP:
        //            return BadRequest("Invalid OTP or admin not found");
        //        case OTPValidationResult.Failure:
        //        default:
        //            return StatusCode(500, "An error occurred while validating OTP");
        //    }
        //}
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateOTP(ValidateOtpDto validateOTP)
        {
            if (validateOTP == null)
            {
                return BadRequest("Invalid OTP data");
            }

            // Use the injected service to validate the OTP
            var result = await _validateOTP.ValidateOTPAsync(validateOTP.OTP);

            // Custom response based on the OTP validation result
            var response = result switch
            {
                OTPValidationResult.Success => new OTPResponse
                {
                    Message = "OTP validated successfully!",
                    Success = true
                },
                OTPValidationResult.InvalidOTP => new OTPResponse
                {
                    Message = "Invalid OTP or admin not found",
                    Success = false
                },
                OTPValidationResult.Failure => new OTPResponse
                {
                    Message = "An error occurred while validating OTP",
                    Success = false
                },
                _ => new OTPResponse
                {
                    Message = "Unknown OTP validation result",
                    Success = false
                }
            };

            return Ok(response);



        }
    }
}
