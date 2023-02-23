using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrbanLife.Data.Enums;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class PurchaseLine
    {
        [Required]
        [ForeignKey(nameof(Purchase))]
        public string PurchaseId { get; set; }

        [Required]
        [ForeignKey(nameof(Line))]
        public string LineId { get; set; }

        [Required]
        public Purchase Purchase { get; set; }

        [Required]
        public Line Line { get; set; }
    }
}