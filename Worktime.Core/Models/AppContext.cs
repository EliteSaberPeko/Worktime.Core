using Microsoft.EntityFrameworkCore;

namespace Worktime.Core.Models
{
    public class AppContext : DbContext
    {
        public DbSet<WTUser> Users { get; set; } = null!;
        public DbSet<WTTask> Tasks { get; set; } = null!;
        public DbSet<WTLine> Lines { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=worktimedb;Username=postgres;Password=postgres");
    }
}
