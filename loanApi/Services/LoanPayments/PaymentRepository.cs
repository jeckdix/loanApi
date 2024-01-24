using loanApi.Data;
using loanApi.Models;



namespace loanApi.Services.LoanPayments
{

        public class PaymentRepository : IPaymentRepository
        
    {
            private readonly DataContext _dataContext;

            public PaymentRepository(DataContext dataContext)
            {
                _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            }

            public async Task<int> MakePaymentAsync(Payment payment)
            {
                if (payment == null)
                {
                    throw new ArgumentNullException(nameof(payment));
                }

        
                if (payment.LoanId <= 0)
                {
                    throw new ArgumentException("Invalid LoanHistoryId");
                }

                _dataContext.Payments.Add(payment);

            try
            {
                await _dataContext.SaveChangesAsync();

                var loanHistory = await _dataContext.loanHistories.FindAsync(payment.LoanId);

                if (loanHistory != null)
                {
                   
                    loanHistory.Balance -= payment.Amount;

                    await _dataContext.SaveChangesAsync();
                }


                return payment.Id;
            }
            catch (Exception)
            {

                throw;
            }
            }


    }
    }

