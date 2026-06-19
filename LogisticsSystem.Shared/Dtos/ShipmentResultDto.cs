namespace LogisticsSystem.Shared.Dtos;

public class ShipmentResultDto
{
    public int Id { get; set; }
    public Guid TrackingId { get; set; }
    public string SenderName { get; set; } = null!;
    public string SenderPhone { get; set; } = null!;
    public string ReceiverName { get; set; } = null!;
    public string ReceiverPhone { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string PackType { get; set; } = null!;
    public decimal Weight { get; set; }
    public string DeliveryAddress { get; set; } = null!;
    public string? ProofImageUrl { get; set; }
    public int? WarehouseId { get; set; }
    public int CustomerId { get; set; }
    public int? DeliveryAgentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TrackingHistoryEntryDto>? TrackingHistory { get; set; }
}