using LogisticsSystem.Shared.Dtos;

namespace LogisticsSystem.Abstraction.Services;

public interface IShipmentService
{
    Task<ShipmentResultDto> CreateShipmentAsync(CreateShipmentDto dto, int customerId);
    Task<ShipmentResultDto?> GetShipmentByIdAsync(int id);
    Task<IEnumerable<ShipmentResultDto>> GetCustomerShipmentsAsync(int customerId);
    Task<ShipmentResultDto?> TrackShipmentByGuidAsync(Guid trackingId);
    Task<IEnumerable<ShipmentResultDto>> GetAgentAssignedShipmentsAsync(int agentId);
    Task<bool> UpdateShipmentStatusAsync(int shipmentId, UpdateStatusDto dto, int updatedBy);
    Task<bool> SubmitDeliveryProofAsync(int shipmentId, SubmitProofDto dto, int updatedBy);
    Task<IEnumerable<ShipmentResultDto>> GetAllShipmentsForAdminAsync();
    Task<bool> AssignWarehouseAsync(int shipmentId, AssignWarehouseDto dto);
    Task<bool> AssignAgentAsync(int shipmentId, AssignAgentDto dto);
}