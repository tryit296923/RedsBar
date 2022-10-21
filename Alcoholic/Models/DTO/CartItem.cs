namespace Alcoholic.Models.DTO
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public string? ProductName { get; set; }
        public int? UnitPrice { get; set; }
        public string? Path { get; set; }
        public float? DiscountAmount { get; set; }

    }
}
