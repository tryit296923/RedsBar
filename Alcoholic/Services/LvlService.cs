using Alcoholic.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Alcoholic.Services
{
    public class LvlService
    {
        private readonly db_a8de26_projectContext db;
        public LvlService(db_a8de26_projectContext db)
        {
            this.db = db;
        }
        public void MemberLvl(string account)
        {
            if (account == "guestonly123")
            {
                return;
            }
            Member? member = (from m in db.Members where m.MemberAccount == account select m).FirstOrDefault();
            if (member == null || member.Orders.FirstOrDefault() == null)
            {
                return;
            }
            int sum = 0;
            foreach (Order order in member.Orders)
            {
                sum += order.Total.GetValueOrDefault();
            }

            int level = 0;
            switch (sum)
            {
                case > 70000: level = 4; break;
                case > 30000: level = 3; break;
                case > 10000: level = 2; break;
                case > 5000: level = 1; break;
                default: level = 0; break;
            }
            member.MemberLevel = level;
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
