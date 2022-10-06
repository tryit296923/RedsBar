using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Employee
    {
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public string EmpAccount { get; set; } = null!;
        public string EmpPassword { get; set; } = null!;
    }
}
