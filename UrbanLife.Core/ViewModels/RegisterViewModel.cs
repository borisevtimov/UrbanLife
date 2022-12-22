using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is not in the correct format!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required!")]
        [MaxLength(20, ErrorMessage = "First name must be less than 20 characters!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [MaxLength(30, ErrorMessage = "Last name must be less than 30 characters!")]
        public string LastName { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
