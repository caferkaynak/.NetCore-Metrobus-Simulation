using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StationApplication.Entity.Entities;

namespace StationApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        public DbSet<SmartTicket> SmartTickets { get; set; }
        public DbSet<SmartTicketType> SmartTicketTypes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationSmartTicket> StationSmartTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StationSmartTicket>()
            .HasOne(p => p.StartStation)
            .WithMany()
            .HasForeignKey(w => w.StartStationId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StationSmartTicket>()
            .HasOne(p => p.FinishStation)
            .WithMany()
            .HasForeignKey(w => w.FinishStationId)
            .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
