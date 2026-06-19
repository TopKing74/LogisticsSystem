using System.Security.Claims;
using LogisticsSystem.Abstraction.Services;
using LogisticsSystem.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShipmentsController(IShipmentService shipmentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateShipmentAsync([FromBody] CreateShipmentDto dto)
    {
        var customerId = GetUserId();
        var result = await shipmentService.CreateShipmentAsync(dto, customerId);
        return Ok(result);
    }

    [HttpGet("my-shipments")]
    public async Task<IActionResult> GetMyShipmentsAsync()
    {
        var customerId = GetUserId();
        var result = await shipmentService.GetCustomerShipmentsAsync(customerId);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetShipmentByIdAsync(int id)
    {
        var result = await shipmentService.GetShipmentByIdAsync(id);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("track/{trackingId:guid}")]
    public async Task<IActionResult> TrackShipmentByGuidAsync(Guid trackingId)
    {
        var result = await shipmentService.TrackShipmentByGuidAsync(trackingId);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "DeliveryAgent")]
    [HttpGet("assigned")]
    public async Task<IActionResult> GetAgentAssignedShipmentsAsync()
    {
        var agentId = GetUserId();
        var result = await shipmentService.GetAgentAssignedShipmentsAsync(agentId);
        return Ok(result);
    }

    [Authorize(Roles = "DeliveryAgent,Admin")]
    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatusAsync(int id, [FromBody] UpdateStatusDto dto)
    {
        var userId = GetUserId();
        var result = await shipmentService.UpdateShipmentStatusAsync(id, dto, userId);
        if (!result)
            return NotFound();
        return Ok(new { message = "Status updated" });
    }

    [HttpPost("{id:int}/proof")]
    public async Task<IActionResult> SubmitProofAsync(int id, [FromBody] SubmitProofDto dto)
    {
        var userId = GetUserId();
        var result = await shipmentService.SubmitDeliveryProofAsync(id, dto, userId);
        if (!result)
            return NotFound();
        return Ok(new { message = "Proof submitted" });
    }

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim!.Value);
    }
}