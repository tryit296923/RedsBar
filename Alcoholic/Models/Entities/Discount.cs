using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public class Discount
    {
        public int DiscountId { get; set; } 
        public int ProductId { get; set; } 
        public string DiscountName { get; set; }
        public float DiscountAmount { get; set; }
        public int Frequency { get; set; }
        public string? Qualify { get; set; }
        public virtual Product Product { get; set; }
    }
}
