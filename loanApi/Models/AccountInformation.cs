using System.ComponentModel.DataAnnotations;

namespace loanApi.Models
{
    public class AccountInformation
    {
        public int Id { get; set; }

        [Required]
        public string AccountName { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string BankName { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Property
        public User User { get; set; }
    }
}
