using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class Stop
    {
        [Key]
        [Column(TypeName = "char(4)")]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [InverseProperty("Stop")]
        public ICollection<Schedule> ArrivingSchedules { get; set; }

        [InverseProperty("NextStop")]
        public ICollection<Schedule> NextStopSchedules { get; set; }

        public Stop()
        {
            ArrivingSchedules = new HashSet<Schedule>();
            NextStopSchedules = new HashSet<Schedule>();
        }
    }
}