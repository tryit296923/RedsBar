using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class BackProdModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int DiscountId { get; set; }
        public string DiscountName { get; set; }
    }
    public class ProductMaterialsModel
    {

        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int MaterialId { get; set; }
        [Required]
        public int Consumption { get; set; }
    }
    public class Materials
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int Consumption { get; set; }
        public string MaterialName { get; set; }
    }
}

