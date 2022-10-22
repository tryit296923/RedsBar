 using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Cost { get; set; }
        public int UnitPrice { get; set; }        
        public string ProductDescription { get; set; }
        public int DiscountId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductsMaterials> ProductsMaterials { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual Discount Discount { get; set; }
    }
}
