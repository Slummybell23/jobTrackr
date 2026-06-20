using System.Security.Claims;
using CachesJobTrackerApi.Controllers.UserController.Models;
using CachesJobTrackerApi.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CachesJobTrackerApi.Controllers.UserController;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await userService.GetCurrentUserAsync(userId);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.User);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await userService.UpdateCurrentUserAsync(userId, request);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.User);
    }
}
