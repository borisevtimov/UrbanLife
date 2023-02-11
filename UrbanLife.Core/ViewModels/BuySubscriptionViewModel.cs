using UrbanLife.Data.Data.Models;
using UrbanLife.Data.Enums;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class BuySubscriptionViewModel
    {
        public SubscriptionType SubscriptionType { get; set; }

        public TimeSpan? TicketStartTime { get; set; }

        public LineType? LineType { get; set; }

        public int? LineNumber { get; set; }

        public string? ChosenLines { get; set; }

        public decimal FinalPrice { get; set; }

        public List<SubscriptionPaymentViewModel> Cards { get; set; }

        public List<Line> Lines { get; set; }
    }
}