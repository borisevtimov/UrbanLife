#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class PaymentInfoViewModel
    {
        public string UserId { get; set; }

        public string AccountFirstName { get; set; }

        public List<PaymentMethodViewModel> Payments { get; set; }

        public string DefaultPaymentNumber { get; set; }
    }
}