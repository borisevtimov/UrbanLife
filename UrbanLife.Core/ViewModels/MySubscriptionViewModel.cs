using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class MySubscriptionViewModel
    {
        public string LineFilter { get; set; }

        public bool? IsValidFilter { get; set; }

        public bool IsPurchaseDateDesc { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public bool HasAllLines { get; set; }

        public List<Line> Lines { get; set; } 

        public List<MySubscriptionDTO> Receipts { get; set; }
    }
}