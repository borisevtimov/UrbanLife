using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не сте въвели имейл адрес!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не сте въвели парола!")]
        public string Password { get; set; }
    }
}