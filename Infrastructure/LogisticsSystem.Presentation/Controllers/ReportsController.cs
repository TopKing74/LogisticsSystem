using LogisticsSystem.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    [HttpGet("daily-shipments")]
    public async Task<IActionResult> GetDailyShipmentsReportAsync()
    {
        var result = await reportService.GetDailyShipmentsReportAsync();
        return Ok(result);
    }

    [HttpGet("warehouse-capacity")]
    public async Task<IActionResult> GetWarehouseCapacityReportAsync()
    {
        var result = await reportService.GetWarehouseCapacityReportAsync();
        return Ok(result);
    }
}