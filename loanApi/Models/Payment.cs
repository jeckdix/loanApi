using System.ComponentModel.DataAnnotations.Schema;

namespace loanApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        // Foreign key for LoanHistory
        public int LoanId { get; set; }
        public LoanHistory LoanHistory { get; set; }

        // Foreign key for User

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
