using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class MonthlyRecord
    {
        public DateTime Year { get; set; }
        public DateTime Month { get; set; }
        public double Percentage { get; set; }
        public int CustNumber { get; set; }
        public string HotProduct1 { get; set; }
        public int Quantity1 { get; set; }
        public string HotProduct2 { get; set; }
        public int Quantity2 { get; set; }
        public string HotProduct3 { get; set; }
        public int Quantity3 { get; set; }
    }
}
