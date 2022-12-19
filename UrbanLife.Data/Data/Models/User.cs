using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }
    }
}