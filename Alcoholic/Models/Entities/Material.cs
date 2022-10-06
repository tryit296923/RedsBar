using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Material
    {
        public string MaterialName { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int Inventory { get; set; }
        public short Cost { get; set; }
        public string ShippingDate { get; set; } = null!;
    }
}
