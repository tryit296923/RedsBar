using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string OrderId { get; set; } = null!;
        public string MemberId { get; set; } = null!;
        public int Number { get; set; }
        public DateTime OrderTime { get; set; }
        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
