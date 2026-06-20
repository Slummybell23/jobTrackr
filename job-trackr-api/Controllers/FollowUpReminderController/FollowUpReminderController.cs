using System.Security.Claims;
using CachesJobTrackerApi.Controllers.FollowUpReminderController.Models;
using CachesJobTrackerApi.Services.FollowUpReminderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CachesJobTrackerApi.Controllers.FollowUpReminderController;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FollowUpReminderController(FollowUpReminderService followUpReminderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReminder([FromBody] CreateFollowUpReminderRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await followUpReminderService.CreateFollowUpReminderAsync(userId, request);
        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Reminder);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListReminders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await followUpReminderService.ListFollowUpRemindersAsync(userId);
        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Reminders);
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> CompleteReminder(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await followUpReminderService.CompleteFollowUpReminderAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Reminder);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReminder(int id, [FromBody] UpdateFollowUpReminderRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await followUpReminderService.UpdateFollowUpReminderAsync(userId, id, request);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Reminder);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReminder(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await followUpReminderService.DeleteFollowUpReminderAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
}
