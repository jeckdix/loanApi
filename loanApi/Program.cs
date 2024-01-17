using loanApi.Data;
using loanApi.Services.LoanType;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<ILoanTypeRepository, LoanTypeRepository>();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ICardDetailsRepository, CardDetailsRepository>();
builder.Services.AddScoped<IAccountInformationRepository, AccountInformationRepository>();
builder.Services.AddScoped<ILoanHistoryRepository, LoanHistoryRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
