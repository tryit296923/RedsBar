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
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
    }

    // 後臺顯示本日訂單
    public class BCOrder
    {
        public string OrderId { get; set; }
        public string MemberName { get; set; }
        public string Desk { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
        public int? Total { get; set; }
        public string OrderTime { get; set; }

    }
    // 後臺輸入訂單編號查詢
    public class SearchHistOrder
    {
        public string OrderId { get; set; }
    }

    //櫃台結帳頁面顯示
    public class FrontDeskCheckPage
    {
        public string OrderId { get; set; }
        public string Desk { get; set; }
        public DateTime OrderTime { get; set; }

    }
}
