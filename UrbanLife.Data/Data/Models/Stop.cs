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

        public ICollection<TimeTable> TimeTables { get; set; }

        public Stop()
        {
            TimeTables = new HashSet<TimeTable>();
        }
    }
}