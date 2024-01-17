using loanApi.Services.LoanType;

namespace loanApi.Helper
{
    public class LoanCalculator
    {
        public static decimal CalculateInterest(decimal loanAmount, double interestRate, int durationMonths)
        {
            double monthlyInterestRate = interestRate / 100 / 12;
            decimal monthlyPayment = loanAmount * (decimal)(monthlyInterestRate / (1 - Math.Pow(1 + monthlyInterestRate, - durationMonths)));
            decimal totalRepayment = monthlyPayment * durationMonths;
            return totalRepayment - loanAmount;
        }

        private bool IsLoanExpiredWithinYears(DateTime? expiryDate, int years)
        {
            // Assume the current date is used for comparison
            if (expiryDate.HasValue && expiryDate.Value.Date < DateTime.UtcNow.Date.AddYears(-years))
            {
                return true; // Loan is expired
            }

            return false; // Loan is not expired
        }
    }
}
