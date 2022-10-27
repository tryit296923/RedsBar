namespace Alcoholic.Models.DTO
{
    public class EditProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Cost { get; set; }
        public int UnitPrice { get; set; }
        public string ProductDescription { get; set; }
        public int DiscountId { get; set; }
    }
}
