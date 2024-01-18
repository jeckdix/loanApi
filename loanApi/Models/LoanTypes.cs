namespace loanApi.Models
{
    public class LoanTypes
    {
        public int Id { get; set; }
        public string? LoanName { get; set; }
        public decimal MaxLoanAmount { get; set;}
        public string? Duration { get; set;}
        public double InterestRate { get; set;}
        public decimal MinLoanAmount { get; set; }

    }
}
