using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.Abstraction.Services;

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseDto>> GetAllWarehousesAsync();
    Task<WarehouseDto?> GetWarehouseByIdAsync(int id);
    Task<WarehouseDto> CreateWarehouseAsync(WarehouseDto dto);
    Task<WarehouseDto?> UpdateWarehouseAsync(int id, WarehouseDto dto);
    Task<bool> DeleteWarehouseAsync(int id);
}