using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Domain.Enums;
using LogisticsSystem.Domain.Models;
using LogisticsSystem.Persistence.Contexts;
using LogisticsSystem.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Service.Services;

public class ShipmentService(ApplicationDbContext context) : IShipmentService
{
    public async Task<ShipmentResultDto> CreateShipmentAsync(CreateShipmentDto dto, int customerId)
    {
        var shipment = new Shipment
        {
            SenderName = dto.SenderName,
            SenderPhone = dto.SenderPhone,
            ReceiverName = dto.ReceiverName,
            ReceiverPhone = dto.ReceiverPhone,
            PackType = dto.PackType,
            Weight = dto.Weight,
            DeliveryAddress = dto.DeliveryAddress,
            Status = ShipmentStatus.Created,
            CreatedAt = DateTime.UtcNow,
            CustomerId = customerId
        };

        context.Shipments.Add(shipment);
        await context.SaveChangesAsync();

        return MapToDto(shipment);
    }

    public async Task<ShipmentResultDto?> GetShipmentByIdAsync(int id)
    {
        var shipment = await context.Shipments.FindAsync(id);
        return shipment is null ? null : MapToDto(shipment);
    }

    public async Task<IEnumerable<ShipmentResultDto>> GetCustomerShipmentsAsync(int customerId)
    {
        var shipments = await context.Shipments
            .Where(s => s.CustomerId == customerId)
            .ToListAsync();

        return shipments.Select(s => MapToDto(s));
    }

    public async Task<ShipmentResultDto?> TrackShipmentByGuidAsync(Guid trackingId)
    {
        var shipment = await context.Shipments
            .Include(s => s.TrackingHistories)
            .FirstOrDefaultAsync(s => s.TrackingId == trackingId);

        return shipment is null ? null : MapToDto(shipment, includeTracking: true);
    }

    public async Task<IEnumerable<ShipmentResultDto>> GetAgentAssignedShipmentsAsync(int agentId)
    {
        var shipments = await context.Shipments
            .Where(s => s.DeliveryAgentId == agentId)
            .ToListAsync();

        return shipments.Select(s => MapToDto(s));
    }

    public async Task<bool> UpdateShipmentStatusAsync(int shipmentId, UpdateStatusDto dto, int updatedBy)
    {
        var shipment = await context.Shipments.FindAsync(shipmentId);
        if (shipment is null)
            return false;

        var newStatus = (ShipmentStatus)dto.StatusValue;

        if (!Enum.IsDefined(typeof(ShipmentStatus), newStatus))
            return false;

        shipment.Status = newStatus;

        context.ShipmentTrackingHistories.Add(new ShipmentTrackingHistory
        {
            ShipmentId = shipmentId,
            Status = newStatus,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = updatedBy
        });

        context.Notifications.Add(new Notification
        {
            UserId = shipment.CustomerId,
            Title = "Shipment Status Updated",
            Body = $"Your shipment {shipment.TrackingId} is now {newStatus}.",
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        });

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubmitDeliveryProofAsync(int shipmentId, SubmitProofDto dto, int updatedBy)
    {
        var shipment = await context.Shipments.FindAsync(shipmentId);
        if (shipment is null)
            return false;

        shipment.ProofImageUrl = dto.ProofImageUrl;
        shipment.Status = ShipmentStatus.Delivered;

        context.ShipmentTrackingHistories.Add(new ShipmentTrackingHistory
        {
            ShipmentId = shipmentId,
            Status = ShipmentStatus.Delivered,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = updatedBy
        });

        context.Notifications.Add(new Notification
        {
            UserId = shipment.CustomerId,
            Title = "Shipment Delivered",
            Body = $"Your shipment {shipment.TrackingId} has been delivered.",
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        });

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ShipmentResultDto>> GetAllShipmentsForAdminAsync()
    {
        var shipments = await context.Shipments.ToListAsync();
        return shipments.Select(s => MapToDto(s));
    }

    public async Task<bool> AssignWarehouseAsync(int shipmentId, AssignWarehouseDto dto)
    {
        var shipment = await context.Shipments.FindAsync(shipmentId);
        if (shipment is null)
            return false;

        var warehouse = await context.Warehouses
            .Include(w => w.Shipments)
            .FirstOrDefaultAsync(w => w.Id == dto.WarehouseId);

        if (warehouse is null)
            return false;

        if (warehouse.Shipments.Count >= warehouse.capacity)
            return false;

        shipment.WarehouseId = dto.WarehouseId;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignAgentAsync(int shipmentId, AssignAgentDto dto)
    {
        var shipment = await context.Shipments.FindAsync(shipmentId);
        if (shipment is null)
            return false;

        shipment.DeliveryAgentId = dto.AgentId;
        await context.SaveChangesAsync();
        return true;
    }

    private static ShipmentResultDto MapToDto(Shipment shipment, bool includeTracking = false)
    {
        return new ShipmentResultDto
        {
            Id = shipment.Id,
            TrackingId = shipment.TrackingId,
            SenderName = shipment.SenderName,
            SenderPhone = shipment.SenderPhone,
            ReceiverName = shipment.ReceiverName,
            ReceiverPhone = shipment.ReceiverPhone,
            Status = shipment.Status.ToString(),
            PackType = shipment.PackType,
            Weight = shipment.Weight,
            DeliveryAddress = shipment.DeliveryAddress,
            ProofImageUrl = shipment.ProofImageUrl,
            WarehouseId = shipment.WarehouseId,
            CustomerId = shipment.CustomerId,
            DeliveryAgentId = shipment.DeliveryAgentId,
            CreatedAt = shipment.CreatedAt,
            TrackingHistory = includeTracking
                ? shipment.TrackingHistories.Select(th => new TrackingHistoryEntryDto
                {
                    Id = th.Id,
                    Status = th.Status.ToString(),
                    UpdatedAt = th.UpdatedAt,
                    UpdatedBy = th.UpdatedBy
                }).ToList()
                : null
        };
    }
}