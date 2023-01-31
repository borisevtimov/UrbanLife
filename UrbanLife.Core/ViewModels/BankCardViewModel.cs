using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class BankCardViewModel
    {
        [Required(ErrorMessage = "Първото име е задължително!")]
        [MaxLength(20, ErrorMessage = "Първото име трябва да е максимално 20 символа!")]
        [Unicode(false)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилията е задължителна!")]
        [MaxLength(30, ErrorMessage = "Първото име трябва да е максимално 30 символа!")]
        [Unicode(false)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Номерът на картата е задължителен!")]
        [RegularExpression(@"^\d{4}(-|\s|)\d{4}(-|\s|)\d{4}(-|\s|)\d{4}$",
            ErrorMessage = "Номерът е в неправилен формат!")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "CVC кодът на картата е задължителен!")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVC кодът трябва да е трицифрен!")]
        public string CVC { get; set; }

        [Required(ErrorMessage = "Дата е задължителна!")]
        public DateTime ExpireDate { get; set; }

        [RegularExpression(@"^(\d+|\d+\.\d{2})$", ErrorMessage = "Невалидна сума!")]
        public decimal? Amount { get; set; } = 0;

        public bool IsDefault { get; set; } = false;
    }
}