using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Material
    {
        public int MaterialId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public int Inventory { get; set; }
        public short Cost { get; set; }
        public DateTime ShippingDate { get; set; }

        public virtual ICollection<ProductsMaterials> ProductsMaterials { get; set; }
        public virtual Category Category { get; set; }

    }
}
