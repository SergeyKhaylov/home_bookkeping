using Microsoft.EntityFrameworkCore;

namespace Homebookkeping
{
    internal class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Transaction> transactions { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=finance;Username=postgres;Password=1234qwerty");
        }
    }
}
