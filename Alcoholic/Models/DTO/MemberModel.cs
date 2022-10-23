using System.ComponentModel.DataAnnotations;

namespace Alcoholic.Models.DTO
{
    public class MemberModel
    {
        [Required, MinLength(8)]
        public string Account { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(10)]
        public string Phone { get; set; }
        [Required]
        public DateTime Birth { get; set; }
    }

    public class LoginViewModel
    {
        [Required, MinLength(8)]
        public string Account { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
