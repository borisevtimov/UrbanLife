using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrbanLife.Data.Enums;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class Schedule
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey(nameof(Line))]
        public string LineId { get; set; }

        [Required]
        [ForeignKey(nameof(Stop))]
        [Column(TypeName = "char(4)")]
        public string StopCode { get; set; }

        [ForeignKey(nameof(NextStopCode))]
        public string? NextStopCode { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan Arrival { get; set; }

        [Required]
        public bool IsWeekday { get; set; } = true;

        [Required]
        public Line Line { get; set; }

        [Required]
        public Stop Stop { get; set; }

        public Stop? NextStop { get; set; }

        public Schedule()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}