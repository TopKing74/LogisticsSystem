using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.Abstraction.Services;

public interface IReportService
{
    Task<IEnumerable<DailyShipmentReportDto>> GetDailyShipmentsReportAsync();
    Task<IEnumerable<WarehouseCapacityReportDto>> GetWarehouseCapacityReportAsync();
}