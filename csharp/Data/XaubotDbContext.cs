using Microsoft.EntityFrameworkCore;
using XaubotClone.Domain;

namespace XaubotClone.Data
{
    public class XaubotDbContext : DbContext
    {
        public XaubotDbContext(DbContextOptions<XaubotDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<TradingActivity> TradingActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Preferences)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TradingActivities)
                .WithOne(ta => ta.User)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure UserPreference entity
            modelBuilder.Entity<UserPreference>()
                .HasIndex(p => new { p.UserId, p.Key })
                .IsUnique();

            // Configure TradingActivity entity
            modelBuilder.Entity<TradingActivity>()
                .Property(ta => ta.Amount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TradingActivity>()
                .Property(ta => ta.EntryPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TradingActivity>()
                .Property(ta => ta.ExitPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TradingActivity>()
                .Property(ta => ta.StopLoss)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TradingActivity>()
                .Property(ta => ta.TakeProfit)
                .HasPrecision(18, 8);
        }
    }
}
