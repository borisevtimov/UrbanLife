using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class Payment
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Column(TypeName = "char(19)")]
        public string Number { get; set; }

        [Required]
        [Unicode(false)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [Unicode(false)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ExpireDate { get; set; }

        [Required]
        [Column(TypeName = "char(3)")]
        public string CVC { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public ICollection<UserPayment> UserPayments { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public Payment()
        {
            Id = Guid.NewGuid().ToString();
            UserPayments = new HashSet<UserPayment>();
            Purchases = new HashSet<Purchase>();
        }
    }
}