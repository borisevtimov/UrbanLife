using System.ComponentModel.DataAnnotations;
using UrbanLife.Data.Enums;
#nullable disable warnings

namespace UrbanLife.Data.Data.Models
{
    public class Line
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Number { get; set; }

        [Required]
        public LineType Type { get; set; }

        public ICollection<Schedule> Schedules { get; set; }

        public Line()
        {
            Id = Guid.NewGuid().ToString();
            Schedules = new HashSet<Schedule>();
        }
    }
}