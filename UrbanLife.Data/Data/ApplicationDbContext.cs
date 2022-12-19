using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrbanLife.Data.Data.Models;
#nullable disable warnings

namespace UrbanLife.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<UserPayment> UserPayments { get; set; }

        public DbSet<Stop> Stops { get; set; }

        public DbSet<Line> Lines { get; set; }

        public DbSet<TimeTable> TimeTables { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseLine> PurchaseLines { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserPayment>()
                .HasKey(up => new { up.UserId, up.PaymentNumber });
        }
    }
}