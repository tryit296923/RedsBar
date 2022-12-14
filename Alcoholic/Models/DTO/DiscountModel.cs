using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class DiscountModel
    {

        public int DiscountId { get; set; }
        [Required]
        public string DiscountName { get; set; }
        [Required]
        public float DiscountAmount { get; set; }
    }
}
