namespace Alcoholic.Models.Entities
{
    public partial class OrderDetail
    {
        public string OrderId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Sum { get; set; }
        public int Total { get; set; }
        public double Discount { get; set; }
        public string? Rate { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
