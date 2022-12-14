using Alcoholic.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class DeskModel
    {
        [Required]
        public int DeskType { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Desk { get; set; }
    }
}
