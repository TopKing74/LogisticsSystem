using LogisticsSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsSystem.Persistence.Configurations;

public class ShipmentTrackingHistoryConfiguration : IEntityTypeConfiguration<ShipmentTrackingHistory>
{
    public void Configure(EntityTypeBuilder<ShipmentTrackingHistory> builder)
    {
        builder.Property(th => th.Status)
            .IsRequired();

        builder.Property(th => th.UpdatedAt)
            .IsRequired();

        builder.Property(th => th.UpdatedBy)
            .IsRequired();

        builder.HasOne<Shipment>()
            .WithMany(s => s.TrackingHistories)
            .HasForeignKey(th => th.ShipmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}