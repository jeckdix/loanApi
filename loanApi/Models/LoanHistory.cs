namespace loanApi.Models
{
    public class LoanHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal LoanAmount { get; set; } = 0;    
        public decimal Balance { get; set; } = 0;
        public string PaymentStatus { get; set; }
        public decimal Interest { get; set; } = 0;
        public bool Disbursed { get; set; } = false;
        public int LoanPackageId { get; set; }
        public int UserId { get; set; }

        // LoanPackages reference
        public LoanTypes LoanPackage { get; set; }

        // User reference
        public User User { get; set; }
        
        public List<Payment> Payments { get; set; }
    }
}
