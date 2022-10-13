using Microsoft.Build.ObjectModelRemoting;

namespace Alcoholic.Models.Entities
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string? DiscountName { get; set; }
        public string? DiscountProduct { get; set; }
        public float DiscountAmount { get; set; }
        public int Frequency { get; set; }
        public string? Qualify { get; set; }

    }
}
