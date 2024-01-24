using loanApi.Data;
using loanApi.Services.LoanType;
using loanApi.Services.OTP;
using loanApi.Services.RegisterUser;
using loanApi.Services.UserLogin;
using loanApi.Services.UserProfileService;
using loanApi.Services.UserRegister;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using loanApi.Services.LoanHistories;
using loanApi.Helper;
using loanApi.Services.LoanPayments;
using loanApi.Services.AccountInformations;
using loanApi.Services.CardDetails;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IUserProfile, UserProfileService>();
builder.Services.AddScoped<IUserLogin, LoginService>();
builder.Services.AddScoped<IValidateOTP, ValidateOtpService>();
builder.Services.AddScoped<ILoanTypeRepository, LoanTypeRepository>();
builder.Services.AddScoped<ILoanHistoryRepository, LoanHistoryRepository>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAccountInformationRepository, AccountInformationRepository>();
builder.Services.AddScoped<ICardDetailsRepository, CardDetailsRepository>();

// Add services to the container.

//Add automapper
builder.Services.AddAutoMapper(typeof(Program));

//Add Memory cache
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization Header Using The Bearer Scheme(\"Bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
