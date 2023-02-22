using UrbanLife.Data.Data.Models;
using UrbanLife.Data.Enums;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class BuySubscriptionViewModel
    {
        public SubscriptionType SubscriptionType { get; set; }

        public LineType? LineType { get; set; }

        public int? LineNumber { get; set; }

        public List<Line> Lines { get; set; }

        public List<SubscriptionPaymentViewModel> Cards { get; set; }

        public decimal FinalPrice { get; set; }

        public string ChosenLines { get; set; }

        public string ChosenCardNumber { get; set; }

        public string ChosenDuration { get; set; }

        public TimeSpan? ChosenTicketStartTime { get; set; }

        public DateTime? ChosenCardStartDate { get; set; }

    }
}