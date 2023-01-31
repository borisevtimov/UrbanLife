using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class UpdateProfileViewModel
    {
        public IFormFile? ProfilePicture { get; set; }

        [EmailAddress(ErrorMessage = "Имейл адресът е с неправилен формат!")]
        public string? Email { get; set; }

        [MaxLength(20, ErrorMessage = "Първото име трябва да е с по-малко от 20 символа!")]
        public string? FirstName { get; set; }

        [MaxLength(30, ErrorMessage = "Фамилията трябва да е с по-малко от 30 символа!")]
        public string? LastName { get; set; }

        [MinLength(6, ErrorMessage = "Паролата трябва да е поне 6 символа!")]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Паролите не съвпадат!")]
        public string? ConfirmPassword { get; set; }
    }
}