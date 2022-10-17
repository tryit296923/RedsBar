using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public partial class Reserves
    {
        public int ReserveId { get; set; }
        public DateTime ReserveDate { get; set; }
        public string ReserveName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public int Number { get; set; }
        public DateTime ReserveSet { get; set; }
    }
}
