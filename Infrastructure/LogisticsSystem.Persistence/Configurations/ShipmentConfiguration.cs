using LogisticsSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Persistence.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.Property(s => s.TrackingId)
            .IsRequired();

        builder.HasIndex(s => s.TrackingId)
            .IsUnique();

        builder.Property(s => s.SenderName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.SenderPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.ReceiverName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.ReceiverPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.PackType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Weight)
            .HasPrecision(18, 2);

        builder.Property(s => s.DeliveryAddress)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(s => s.ProofImageUrl)
            .IsRequired(false);

        builder.Property(s => s.Status)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.HasOne(s => s.Warehouse)
            .WithMany(w => w.Shipments)
            .HasForeignKey(s => s.WarehouseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Customer)
            .WithMany(u => u.CreatedShipments)
            .HasForeignKey(s => s.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.DeliveryAgent)
            .WithMany(u => u.DeliveredShipments)
            .HasForeignKey(s => s.DeliveryAgentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.TrackingHistories)
            .WithOne()
            .HasForeignKey(th => th.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}