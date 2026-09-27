using System.Threading.RateLimiting;
using DomainModel;
using Microsoft.EntityFrameworkCore;
namespace DBLayer
{
    public class MSSQLContext: DbContext  
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<AppInit> AppInit { get; set; }
        public MSSQLContext() {}
        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Client>().HasIndex(c => c.name).IsUnique();
            builder.Entity<User>().HasIndex(u => u.id).IsUnique();
        }

    }
}
