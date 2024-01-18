using loanApi.Models;
using Microsoft.EntityFrameworkCore;

namespace loanApi.Data
{
    public class DataContext : DbContext

    {
        public DataContext() : base()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }




        public DbSet<RegisterUsers> userRegister { get; set; }

        //Line 12 above for cards model
        public DbSet<CardDetail> cardDetails { get; set; }
        //Line 14 above for Account info model
        public DbSet<AccountInformation> accountInformations { get; set; }
        //line 16 above for Loan model 

        //Line 14 above for Account info model
        public DbSet<LoanTypes> Loantypes { get; set; }
        //line 16 above for Loan model 


        public DbSet<LoanHistory> loanHistories { get; set; }
    }

}


