using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Member
    {
        public Guid MemberID { get; set; } 
        public string MemberAccount { get; set; }
        public string MemberPassword { get; set; }
        public int MemberLevel { get; set; }
        public string Salt { get; set; } 
        public string MemberName { get; set; }
        public DateTime MemberBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public Guid? EmailID { get; set; }
        public string? Qualified { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
