using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public partial class Order
    {
        public string OrderId { get; set; }
        public Guid MemberId { get; set; }
        public int Number { get; set; }
        public DateTime OrderTime { get; set; }
        public string DeskNum { get; set; }
        public string Status { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Feedback Feedback { get; set; }
    }
}
