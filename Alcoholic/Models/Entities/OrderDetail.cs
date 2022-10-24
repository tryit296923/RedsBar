using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.Entities
{
    public partial class OrderDetail
    {
        public string OrderId { get; set; }
        public int Sequence { get; set; }
        public int ProductId { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public float Discount { get; set; }
        public string? Rate { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
