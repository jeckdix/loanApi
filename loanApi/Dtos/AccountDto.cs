using System.ComponentModel.DataAnnotations;

namespace loanApi.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }

        [Required]
        public string AccountName { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string BankName { get; set; }
        public int UserId { get; set; }
    }
}
