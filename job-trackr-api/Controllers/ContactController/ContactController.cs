using System.Security.Claims;
using CachesJobTrackerApi.Controllers.ContactController.Models;
using CachesJobTrackerApi.Services.ContactService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CachesJobTrackerApi.Controllers.ContactController;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ContactController(ContactService contactService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await contactService.CreateContactAsync(userId, request);
        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Contact);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListContacts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await contactService.ListContactsAsync(userId);
        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Contacts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetContact(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await contactService.GetContactAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Contact);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] UpdateContactRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await contactService.UpdateContactAsync(userId, id, request);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Contact);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await contactService.DeleteContactAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
}
