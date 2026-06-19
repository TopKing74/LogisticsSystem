namespace LogisticsSystem.Shared.Dtos;

public class CreateShipmentDto
{
    public string SenderName { get; set; } = null!;
    public string SenderPhone { get; set; } = null!;
    public string ReceiverName { get; set; } = null!;
    public string ReceiverPhone { get; set; } = null!;
    public string PackType { get; set; } = null!;
    public decimal Weight { get; set; }
    public string DeliveryAddress { get; set; } = null!;
}