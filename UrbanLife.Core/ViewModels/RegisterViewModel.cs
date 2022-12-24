using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имейл адресът е задължителен!")]
        [EmailAddress(ErrorMessage = "Имейл адресът е с неправилен формат!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Първото име е задължително!")]
        [MaxLength(20, ErrorMessage = "Първото име трябва да е с по-малко от 20 символа!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилията е задължителна!")]
        [MaxLength(30, ErrorMessage = "Фамилията трябва да е с по-малко от 30 символа!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна!")]
        [MinLength(6, ErrorMessage = "Паролата трябва да е поне 6 символа!")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Паролите не съвпадат!")]
        public string? ConfirmPassword { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        [Range(0, 0, ErrorMessage = "Имейлът адресът вече е зает!")]
        public int EmailAlreadyUsed { get; set; }
    }
}
