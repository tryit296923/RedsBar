﻿using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Cost { get; set; }
        public int UnitPrice { get; set; }        
        public int SaleAt { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Discount> Discount { get; set; }
        public virtual ICollection<ProductsMaterials> ProductsMaterials { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
    }
}
