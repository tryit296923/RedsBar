using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public partial class Employee
    {
        public Guid EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpAccount { get; set; }
        public string EmpPassword { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
    }
}
