using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class UserPayment
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [ForeignKey(nameof(Payment))]
        public string PaymentNumber { get; set; }

        [Required]
        public Payment Payment { get; set; }
    }
}