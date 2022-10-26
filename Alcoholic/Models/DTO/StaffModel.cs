using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class StaffModel
    {
        [Required]
        public string EmpName { get; set; }
        [Required]
        public string EmpAccount { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
