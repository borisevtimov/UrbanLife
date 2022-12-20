using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Unicode(false)]
        [MaxLength(50)]
        public string ProfileImageName { get; set; }

        public ICollection<UserPayment> UserPayments { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public User()
        {
            UserPayments = new HashSet<UserPayment>();
            Purchases = new HashSet<Purchase>();
        }
    }
}