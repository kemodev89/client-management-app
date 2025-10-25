using ClientDashboardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientDashboardAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Client>()
             .HasMany(c => c.PhoneNumbers)
             .WithOne(p => p.Client)
             .HasForeignKey(p => p.ClientId)
             .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Client>().Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            b.Entity<Client>().Property(x => x.LastName).HasMaxLength(100).IsRequired();
            b.Entity<PhoneNumber>().Property(x => x.Number).HasMaxLength(32).IsRequired();
        }
    }
}
