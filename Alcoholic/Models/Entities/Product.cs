using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Cost { get; set; }
        public int UnitPrice { get; set; }
        //public string ImgPath { get; set; }
        public int SaleAt { get; set; }
        public int? Vodka { get; set; }
        public int? Gin { get; set; }
        public int? Wiskey { get; set; }
        public int? Tequila { get; set; }
        public int? Rum { get; set; }
        public int? Cointreau { get; set; }
        public int? Vermouth { get; set; }
        public int? LemonNade { get; set; }
        public int? Sugar { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
