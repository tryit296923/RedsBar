using Alcoholic.Models.Entities;

namespace Alcoholic.Models.RequestModels
{
    public class OrderRequestModel : Product
    {
        public string OrderId { get; set; }
        public string MemberId { get; set; } = null!;
        public int Number { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public double Discount { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }
    }
}
