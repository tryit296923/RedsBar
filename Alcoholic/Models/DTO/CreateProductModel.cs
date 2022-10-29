using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class CreateProductModel
    {
        [Required]
        public string ProductName {get;set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public int UnitPrice { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public List<IFormFile> Images { get; set; }
        public int DiscountId { get; set; }

    }
}
