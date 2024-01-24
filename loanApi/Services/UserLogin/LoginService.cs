using loanApi.Data;
using loanApi.Dtos;
using loanApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace loanApi.Services.UserLogin
{
    public class LoginService : IUserLogin
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        public async Task<string> AuthenticateUserAsync(LoginDto loginuser)
        {
            var NewLogin = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginuser.Email);

            if (NewLogin == null)
            {
                return null; // User not found
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginuser.Password, NewLogin.Password);

            if (!isPasswordValid)
            {
                return null; // Invalid credentials
            }

            // If credentials are valid, create and return a JWT token
            return await CreateToken(NewLogin);
        }


        private async Task<string> CreateToken(Models.User registerUsers)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == registerUsers.Email);

            List<Claim> claims = new List<Claim>
        {
          
            new Claim(ClaimTypes.Email, registerUsers.Email),
            new Claim(ClaimTypes.Role, "User"),
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())


        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
