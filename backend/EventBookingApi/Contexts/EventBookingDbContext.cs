using Microsoft.EntityFrameworkCore;
using EventBookingApi.Models;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Contexts
{
    public class EventBookingDbContext : DbContext
    {
        public EventBookingDbContext(DbContextOptions<EventBookingDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id).HasName("PK_User");

                user.Property(u => u.Role).HasConversion<string>();

                user.HasMany(u => u.Bookings)
                    .WithOne(b => b.User)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Booking_User");

                user.HasMany(u => u.RefreshTokens)
                    .WithOne(rt => rt.User)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_RefreshToken_User");
            });

            // RefreshToken
            modelBuilder.Entity<RefreshToken>(rt =>
            {
                rt.HasKey(t => t.Id).HasName("PK_RefreshToken");
            });

            // Category
            modelBuilder.Entity<Category>(cat =>
            {
                cat.HasKey(c => c.Id).HasName("PK_Category");

                cat.HasMany(c => c.Events)
                    .WithOne(e => e.Category)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Event_Category");
            });

            // Location
            modelBuilder.Entity<Location>(loc =>
            {
                loc.HasKey(l => l.Id).HasName("PK_Location");

                loc.HasMany(l => l.Events)
                    .WithOne(e => e.Location)
                    .HasForeignKey(e => e.LocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Event_Location");
            });

            // Event
            modelBuilder.Entity<Event>(ev =>
            {
                ev.HasKey(e => e.Id).HasName("PK_Event");

                ev.HasMany(e => e.Bookings)
                    .WithOne(b => b.Event)
                    .HasForeignKey(b => b.EventId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Booking_Event");
            });

            // Booking
            modelBuilder.Entity<Booking>(booking =>
            {
                booking.HasKey(b => b.Id).HasName("PK_Booking");

                booking.HasIndex(b => new { b.UserId, b.EventId })
                    .IsUnique()
                    .HasDatabaseName("UX_User_Event_Booking");
            });

            modelBuilder.Entity<BookingIdResult>().HasNoKey();
        }
    }
}
