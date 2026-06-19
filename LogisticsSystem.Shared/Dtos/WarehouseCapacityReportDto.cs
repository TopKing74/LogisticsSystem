namespace LogisticsSystem.Shared.Dtos;

public class WarehouseCapacityReportDto
{
    public int WarehouseId { get; set; }
    public string Name { get; set; } = null!;
    public int UsedCapacity { get; set; }
    public int TotalCapacity { get; set; }
}