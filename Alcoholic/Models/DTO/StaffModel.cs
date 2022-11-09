using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class StaffModel
    {
        [Required]
        public string EmpName { get; set; }
        [Required, MinLength(8)]
        public string EmpAccount { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required, MinLength(10), MaxLength(10)]
        public string Contact { get; set; }
        [Required]
        public string Role { get; set; }
        public int? Salary { get; set; }
        public DateTime? Join { get; set; }
        public DateTime? Leave { get; set; }
        public int? Status { get; set; }
    }

    public class StaffRegisterModel
    {
        public string EmpName { get; set; }
        public string EmpAccount { get; set; }
        public string EmpPassword { get; set; }
        public string NickName { get; set; }
        public string Contact { get; set; }
        public string Role { get; set; }
        public int? Salary { get; set; }
        public DateTime? Join { get; set; }
        public int? Status { get; set; }
    }

    public class StaffLoginModel
    {
        [Required]
        public string EmpAccount { get; set; }
        [Required]
        public string EmpPassword { get; set; }
    }
}
