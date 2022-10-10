using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Reserf
    {
        public int ReserveId { get; set; }
        public DateTime ReserveDate { get; set; }
        public string ReserveName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public int Number { get; set; }
        public DateTime ReserveSet { get; set; }
    }
}
