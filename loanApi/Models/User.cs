using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace loanApi.Models
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; } 
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string OTP {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public UserProfile Profile { get; set; }

        public List<Payment> Payments { get; set; }
    }
}
