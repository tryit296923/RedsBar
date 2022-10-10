using System;
using System.Collections.Generic;

namespace Alcoholic.Models.Entities
{
    public partial class Member
    {
        public Member()
        {
            Orders = new HashSet<Order>();
        }

        public string MemberID { get; set; } = null!;
        public string MemberAccount { get; set; } = null!;
        public string MemberPassword { get; set; } = null!;
        public string MemberName { get; set; } = null!;
        public DateTime MemberBirth { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public Guid? EmailID { get; set; }
        public string? Qualified { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
