using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Persistence.Contexts;
using LogisticsSystem.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Service.Services;

public class ReportService(ApplicationDbContext context) : IReportService
{
    public async Task<IEnumerable<DailyShipmentReportDto>> GetDailyShipmentsReportAsync()
    {
        return await context.Shipments
            .GroupBy(s => s.CreatedAt.Date)
            .Select(g => new DailyShipmentReportDto
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(d => d.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<WarehouseCapacityReportDto>> GetWarehouseCapacityReportAsync()
    {
        return await context.Warehouses
            .Include(w => w.Shipments)
            .Select(w => new WarehouseCapacityReportDto
            {
                WarehouseId = w.Id,
                Name = w.Name,
                UsedCapacity = w.Shipments.Count,
                TotalCapacity = w.capacity
            })
            .ToListAsync();
    }
}