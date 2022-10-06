using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alcoholic.Models.Entities
{
    public class MembersModel
    {
        [Key()]
        public string? MemberID { get; set; }
        public string? MemberAccount { get; set; }
        public string? MemberPassword { get; set; }
        public string? MemberName { get; set; }
        public DateTime? MemberBirth { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
