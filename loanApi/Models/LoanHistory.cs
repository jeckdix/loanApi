namespace loanApi.Models
{
    public class LoanHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public int LoanPackageId { get; set; }
        public int UserId { get; set; }

        // LoanPackages reference
        public LoanTypes LoanPackage { get; set; }

        // User reference
        public User User { get; set; }

        // Collection of payments associated with this loan
        //public List<Payment> Payments { get; set; }
    }
}
