using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.Entities
{
    public class DeskInfo
    {
        [Key]
        public string? Desk { get; set; }
        public string? StartTime { get; set; }       
        public string? Number { get; set; }
        public string? EndTime { get; set; }
        public int Occupied { get; set; }

    }
}
