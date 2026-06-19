using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Domain.Models;
using LogisticsSystem.Persistence.Contexts;
using LogisticsSystem.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Service.Services;

public class WarehouseService(ApplicationDbContext context) : IWarehouseService
{
    public async Task<IEnumerable<WarehouseDto>> GetAllWarehousesAsync()
    {
        return await context.Warehouses
            .Include(w => w.Shipments)
            .Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Capacity = w.capacity,
                CurrentShipmentsCount = w.Shipments.Count
            })
            .ToListAsync();
    }

    public async Task<WarehouseDto?> GetWarehouseByIdAsync(int id)
    {
        return await context.Warehouses
            .Include(w => w.Shipments)
            .Where(w => w.Id == id)
            .Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Capacity = w.capacity,
                CurrentShipmentsCount = w.Shipments.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<WarehouseDto> CreateWarehouseAsync(WarehouseDto dto)
    {
        var warehouse = new Warehouse
        {
            Name = dto.Name,
            capacity = dto.Capacity
        };

        context.Warehouses.Add(warehouse);
        await context.SaveChangesAsync();

        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Capacity = warehouse.capacity,
            CurrentShipmentsCount = 0
        };
    }

    public async Task<WarehouseDto?> UpdateWarehouseAsync(int id, WarehouseDto dto)
    {
        var warehouse = await context.Warehouses
            .Include(w => w.Shipments)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (warehouse is null)
            return null;

        warehouse.Name = dto.Name;
        warehouse.capacity = dto.Capacity;

        await context.SaveChangesAsync();

        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Capacity = warehouse.capacity,
            CurrentShipmentsCount = warehouse.Shipments.Count
        };
    }

    public async Task<bool> DeleteWarehouseAsync(int id)
    {
        var warehouse = await context.Warehouses.FindAsync(id);
        if (warehouse is null)
            return false;

        context.Warehouses.Remove(warehouse);
        await context.SaveChangesAsync();
        return true;
    }
}