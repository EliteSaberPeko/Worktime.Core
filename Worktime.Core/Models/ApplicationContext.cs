using Microsoft.EntityFrameworkCore;

namespace Worktime.Core.Models
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<WTUser> Users { get; set; } = null!;
        public DbSet<WTTask> Tasks { get; set; } = null!;
        public DbSet<WTLine> Lines { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!Configuration.GetConnectionString(out string connectionString, out string error))
            {
                throw new Exception(error);
            }
            optionsBuilder.UseNpgsql(connectionString);
        }
            
    }
}
