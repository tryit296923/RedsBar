namespace Alcoholic.Models.DTO
{
    public class CreateProductModel
    {
        public string ProductName {get;set; }
        public int Cost { get; set; }
        public int UnitPrice { get; set; }
        public string ProductDescription { get; set; }
        public List<IFormFile> Images { get; set; }

    }
}
