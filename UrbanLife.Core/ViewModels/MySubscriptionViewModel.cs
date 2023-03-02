using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Core.ViewModels
{
    public class MySubscriptionViewModel
    {
        public int TotalPages { get; set; }

        public bool HasAllLines { get; set; }

        public List<Line> Lines { get; set; } 

        public List<MySubscriptionDTO> Receipts { get; set; }
    }
}