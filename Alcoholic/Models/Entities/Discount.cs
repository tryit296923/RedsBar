namespace Alcoholic.Models.Entities
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string? DiscountName { get; set; }
        public string? DiscountType { get; set; }
        public float DiscountAmount { get; set; }
        public int Duration { get; set; }
    }
}
