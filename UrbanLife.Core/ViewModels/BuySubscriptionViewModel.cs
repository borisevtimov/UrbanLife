using UrbanLife.Data.Enums;

namespace UrbanLife.Core.ViewModels
{
    public class BuySubscriptionViewModel
    {
        public PurchaseType PurchaseType { get; set; }

        public TimeSpan? TicketStartTime { get; set; }

        public LineType LineType { get; set; }

        public int LineNumber { get; set; }
    }
}