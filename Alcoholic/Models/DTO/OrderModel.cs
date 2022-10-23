using Alcoholic.Models.Entities;

namespace Alcoholic.Models.DTO
{
    public class OrderViewModel
    {
        public string MemberName { get; set; }
        public List<CartItem> ItemList { get; set; }
        public string Number { get; set; }
        public int Desk { get; set; }
        public string? OrderId { get; set; }
        public DateTime? OrderTime { get; set; }
        public string? Status { get; set; } = "N";
    }
    //public class OrderTotalViewModel
    //{
    //    //存入資料庫Order
    //    public List<OrderListViewModel> Orders { get; set; }
    //    //存入資料庫OrderDetail
    //    public List<CartItem> Details { get; set; }
    //}
    //public class OrderListViewModel
    //{
    //    public string OrderId { get; set; }
    //    public Guid MemberId { get; set; }
    //    public string MemberName { get; set; }
    //    public int Number { get; set; }
    //    public DateTime OrderTime { get; set; }
    //    public string Desk { get; set; }
    //    public string Status { get; set; } = "N";
    //}
}
