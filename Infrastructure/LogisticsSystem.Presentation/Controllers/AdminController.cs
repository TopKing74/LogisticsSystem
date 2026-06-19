using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IShipmentService shipmentService) : ControllerBase
{
    [HttpGet("shipments")]
    public async Task<IActionResult> GetAllShipmentsAsync()
    {
        var result = await shipmentService.GetAllShipmentsForAdminAsync();
        return Ok(result);
    }

    [HttpPut("shipments/{id:int}/assign-warehouse")]
    public async Task<IActionResult> AssignWarehouseAsync(int id, [FromBody] AssignWarehouseDto dto)
    {
        var result = await shipmentService.AssignWarehouseAsync(id, dto);
        if (!result)
            return NotFound();
        return Ok(new { message = "Warehouse assigned" });
    }

    [HttpPut("shipments/{id:int}/assign-agent")]
    public async Task<IActionResult> AssignAgentAsync(int id, [FromBody] AssignAgentDto dto)
    {
        var result = await shipmentService.AssignAgentAsync(id, dto);
        if (!result)
            return NotFound();
        return Ok(new { message = "Agent assigned" });
    }
}