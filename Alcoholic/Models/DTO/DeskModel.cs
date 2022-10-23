using Alcoholic.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class DeskModel
    {
        [Required, MaxLength(1)]
        public int DeskType { get; set; }
        [Required,MaxLength(1)]
        public int Number { get; set; }
        [Required, MaxLength(2)]
        public int Desk { get; set; }
    }
}
