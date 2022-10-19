using Alcoholic.Models.Entities;

namespace Alcoholic.Models.RequestModels
{
    public class OrderRequestModel : Product
    {
        public Guid OrderId { get; set; }
        public Guid MemberId { get; set; }
        public int Number { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; }
    }
}
