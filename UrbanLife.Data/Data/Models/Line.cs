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

        public ICollection<TimeTable> TimeTables { get; set; }

        public Line()
        {
            Id = Guid.NewGuid().ToString();
            TimeTables = new HashSet<TimeTable>();
        }
    }
}