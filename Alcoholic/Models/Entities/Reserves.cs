using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public partial class Reserves
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ReserveId { get; set; }
        public DateTime ReserveDate { get; set; }
        public string ReserveName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public int Number { get; set; }
        public DateTime ReserveSet { get; set; }
    }
}
