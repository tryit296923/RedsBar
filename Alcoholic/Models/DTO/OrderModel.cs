using Alcoholic.Models.Entities;

namespace Alcoholic.Models.DTO
{
    public class OrderViewModel
    {
        public string MemberName { get; set; }
        public List<CartItem> ItemList { get; set; }
        public int Number { get; set; }
        public string Desk { get; set; }
        public string? OrderId { get; set; }
        public DateTime? OrderTime { get; set; }
        public string? Status { get; set; } = "N";
    }

    public class OrderCheckViewModel
    {
        public string Number { get; set; }
        public string Desk { get; set; }
        public string OrderId { get; set; }
        public string OrderTime { get; set; }
        public int Total { get; set; }
        public List<CartTotal> CartTotal { get; set; }

    }

    public class CartTotal
    {
        public CartIdNamePair IdNamePair { get; set; }
        public int Count { get; set; }
    }

    public class CartIdNamePair
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    // 後臺顯示本日訂單
    public class BCOrder
    {
        public string OrderId { get; set; }
        public string MemberName { get; set; }
        public string Desk { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
    }
    
}
