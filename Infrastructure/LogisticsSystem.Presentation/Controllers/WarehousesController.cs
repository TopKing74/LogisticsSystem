using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController(IWarehouseService warehouseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllWarehousesAsync()
    {
        var result = await warehouseService.GetAllWarehousesAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWarehouseByIdAsync(int id)
    {
        var result = await warehouseService.GetWarehouseByIdAsync(id);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateWarehouseAsync([FromBody] WarehouseDto dto)
    {
        var result = await warehouseService.CreateWarehouseAsync(dto);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateWarehouseAsync(int id, [FromBody] WarehouseDto dto)
    {
        var result = await warehouseService.UpdateWarehouseAsync(id, dto);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWarehouseAsync(int id)
    {
        var result = await warehouseService.DeleteWarehouseAsync(id);
        if (!result)
            return NotFound();
        return Ok(new { message = "Warehouse deleted" });
    }
}