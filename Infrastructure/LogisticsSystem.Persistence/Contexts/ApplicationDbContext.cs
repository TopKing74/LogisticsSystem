using LogisticsSystem.Domain.Models;
using LogisticsSystem.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<ShipmentTrackingHistory> ShipmentTrackingHistories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}