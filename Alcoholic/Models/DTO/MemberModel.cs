using System.Security.Permissions;
using System.Security.Policy;

namespace Alcoholic.Models.DTO
{
    public class MemberModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Birth { get; set; }
    }

    public class LoginViewModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
