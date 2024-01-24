
using loanApi.Models;

namespace loanApi.Services.LoanPayments
{
    public interface IPaymentRepository
    {
        Task<int> MakePaymentAsync(Payment payment);
    }
}
