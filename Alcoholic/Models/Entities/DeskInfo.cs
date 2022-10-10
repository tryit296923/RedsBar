using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.Entities
{
    public class DeskInfo
    {
        [Key]
        public string? Desk { get; set; }
        public string? Number { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

    }
}
