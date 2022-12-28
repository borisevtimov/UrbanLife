#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class PaymentMethodViewModel
    {
        public string CardNumber { get; set; }

        public string CardFirstName { get; set; }

        public string CardLastName { get; set; }

        public DateTime ExpireDate { get; set; }

        public string CVC { get; set; }

        public bool IsDefault { get; set; }
    }
}