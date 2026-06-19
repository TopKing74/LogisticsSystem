namespace LogisticsSystem.Shared.Dtos;

public class WarehouseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public int CurrentShipmentsCount { get; set; }
}