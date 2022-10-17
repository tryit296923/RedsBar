using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [MaxLength(10), Column(TypeName = "varchar")]

        public string Desk { get; set; }

        public DateTime OrderTime { get; set; }
        [MaxLength(1), Column(TypeName = "char")]
        public string Status { get; set; } = "N";
        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
