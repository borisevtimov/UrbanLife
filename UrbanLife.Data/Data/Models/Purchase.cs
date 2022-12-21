﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class Purchase
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Column(TypeName = "char(19)")]
        [ForeignKey(nameof(Payment))]
        public string? PaymentNumber { get; set; }

        public decimal? Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public bool IsCard { get; set; } = true;

        [Required]
        public User User { get; set; }

        public Payment? Payment { get; set; }

        public ICollection<PurchaseLine> PurchaseLines { get; set; }

        public Purchase()
        {
            Id = Guid.NewGuid().ToString();
            PurchaseLines = new HashSet<PurchaseLine>();
        }
    }
}