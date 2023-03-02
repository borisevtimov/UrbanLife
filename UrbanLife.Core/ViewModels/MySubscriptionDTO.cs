using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class MySubscriptionDTO
    {
        public string PurchaseId { get; set; }

        public List<Line> Lines { get; set; }

        public DateTime PurchaseDate { get; set; }

        public bool IsValid { get; set; }
    }
}