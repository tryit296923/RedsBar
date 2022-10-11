using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Reserves
    {
        public int ReserveId { get; set; }
        public DateTime ReserveDate { get; set; }
        public string ReserveName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public int Number { get; set; }
        public DateTime ReserveSet { get; set; }
    }
}
