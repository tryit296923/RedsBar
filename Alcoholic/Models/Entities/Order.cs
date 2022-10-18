using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Order
    {
        public Guid OrderId { get; set; }
        public Guid MemberId { get; set; }
        public int Number { get; set; }
        public DateTime OrderTime { get; set; }
        public virtual Member Member { get; set; }
        public virtual Feedback Feedback { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
