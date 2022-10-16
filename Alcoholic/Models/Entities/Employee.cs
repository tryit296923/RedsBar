using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public partial class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public string EmpAccount { get; set; } = null!;
        public string EmpPassword { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
