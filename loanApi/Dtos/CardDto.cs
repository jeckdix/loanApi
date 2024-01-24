using System.ComponentModel.DataAnnotations;

namespace loanApi.Dtos
{
    public class CardDto
    {
        public int Id { get; set; }

        [Required]
        public string CardName { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public string CVV { get; set; }
        public int UserId { get; set; }
    }
}
