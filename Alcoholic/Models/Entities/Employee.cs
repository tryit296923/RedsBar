using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.Entities
{
    public partial class Employee
    {
        [Required]
        public Guid EmpId { get; set; }
        [Required]
        public string EmpName { get; set; }
        [Required]
        public string EmpAccount { get; set; }
        [Required]
        public string EmpPassword { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public int Salary { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public DateTime Join { get; set; }
        public int Status { get; set; }
        public DateTime? Leave { get; set; }
    }
}
