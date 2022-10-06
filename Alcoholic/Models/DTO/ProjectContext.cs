using Alcoholic.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Alcoholic.Models.DTO
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() 
        { 
        }
        public ProjectContext(DbContextOptions<ProjectContext> option):base(option) 
        { 

        }
        public DbSet<MembersModel> Members { get; set; }
    }
}
