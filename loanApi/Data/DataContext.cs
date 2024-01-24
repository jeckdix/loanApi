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




        public DbSet<User> Users { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        //Line 12 above for cards model
        public DbSet<CardDetail> cardDetails { get; set; }
        //Line 14 above for Account info model
        public DbSet<AccountInformation> accountInformations { get; set; }
        //line 16 above for Loan model 

        //Line 14 above for Account info model
        public DbSet<LoanTypes> Loantypes { get; set; }
        //line 16 above for Loan model 


        public DbSet<LoanHistory> loanHistories { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.MaritalStatus)
                .HasConversion<string>();
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)  
                .WithMany(u => u.Payments)  
                .HasForeignKey(p => p.UserId) 
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.LoanHistory)   
                .WithMany(lh => lh.Payments)   
                .HasForeignKey(p => p.LoanId)  
                .OnDelete(DeleteBehavior.Restrict);  

            base.OnModelCreating(modelBuilder);
        }
    }

}


