using System.Security.Claims;
using LogisticsSystem.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserNotificationsAsync()
    {
        var userId = GetUserId();
        var result = await notificationService.GetUserNotificationsAsync(userId);
        return Ok(result);
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsReadAsync(int id)
    {
        var result = await notificationService.MarkAsReadAsync(id);
        if (!result)
            return NotFound();
        return Ok(new { message = "Notification marked as read" });
    }

    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim!.Value);
    }
}