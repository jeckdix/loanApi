using Microsoft.EntityFrameworkCore;

namespace loanApi.Data
{
    public class DataContext : DbContext

    {
        public DataContext(DbContextOptions <DataContext> options) : base(options){}


        //Line 10 above for user model

        //Line 12 above for cards model

        //Line 14 above for Account info model

        //line 16 above for Loan model 

    }
}


