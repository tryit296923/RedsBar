using Alcoholic.Models.Entities;
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

    public class DataPageModel
    {
        public string account { get; set; }
        public string name { get; set; }
        public DateTime birth { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public int total { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public float discount { get; set; }
        public List<Product> products { get; set; }
        //public List<string> path { get; set; }

    }
}
