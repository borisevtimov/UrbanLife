#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class PaymentMethodViewModel
    {
        public string PaymentId { get; set; }
        
        public string CardNumber { get; set; }

        public string CardFirstName { get; set; }

        public string CardLastName { get; set; }

        public DateTime ExpireDate { get; set; }

        public string CVC { get; set; }

        public bool IsDefault { get; set; }

        public decimal Amount { get; set; }
    }
}