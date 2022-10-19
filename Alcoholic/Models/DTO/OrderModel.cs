using Alcoholic.Models.Entities;

namespace Alcoholic.Models.DTO
{
    public class OrderViewModel
    {
        public List<OrderRequestModel> ItemList { get; set; }
        
    }
    public class OrderRequestModel
    {
        public string OrderId { get; set; }
        public Guid MemberId { get; set; }
        public int ProductId { get; set; }
        public int Number { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }
    }
    public class OrderTotalViewModel
    {
        public List<OrderListViewModel> Orders { get; set; }
        public List<DetailViewModel> Details { get; set; }
    }
    public class OrderListViewModel
    {
        public string OrderId { get; set; }
        public Guid MemberId { get; set; }
        public int Number { get; set; }
        public DateTime OrderTime { get; set; }
        public string DeskNum { get; set; }
        public string Status { get; set; } = "N";
    }
    public class DetailViewModel
    {
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Path { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public double Discount { get; set; }
    }
}
