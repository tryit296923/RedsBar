using Alcoholic.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.Entities
{
    public class DeskInfo
    {
        public int DeskId { get; set; }
        public DeskTypeEnum DeskType { get; set; }
        public string Desk { get; set; }
        public int Number { get; set; }
        public string? StartTime { get; set; }       
        public string? EndTime { get; set; }
        public int Occupied { get; set; }
    }
}