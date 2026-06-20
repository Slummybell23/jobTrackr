using System.Security.Claims;
using CachesJobTrackerApi.Controllers.ResumeController.Models;
using CachesJobTrackerApi.Services.ResumeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CachesJobTrackerApi.Controllers.ResumeController;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ResumeController(ResumeService resumeService) : ControllerBase
{
    private const long MaxFileSizeBytes = 1024L * 1024 * 1024; // 1 GB

    [HttpPost("upload")]
    [RequestSizeLimit(MaxFileSizeBytes)]
    public async Task<IActionResult> UploadResume([FromForm] CreateResumeRq request)
    {
        // The authenticated user's id comes from the bearer token's NameIdentifier claim.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await resumeService.UploadResumeAsync(userId, request);
        if (!result.Success)
            return BadRequest(result.Error);

        var resume = result.Resume!;
        return Ok(new CreateResumeRs
        {
            ResumeId = resume.ResumeId,
            ResumeName = resume.ResumeName,
            ResumeFilePath = resume.ResumeFilePath,
            ResumeCreatedDate = resume.ResumeCreatedDate
        });
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListResumes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var resumes = await resumeService.ListResumeAsync(userId);
        if(resumes.Success)
            return Ok(resumes.Resumes);
        
        return BadRequest(resumes.Error);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetResume(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await resumeService.GetResumeAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Resume);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateResume(int id, [FromBody] UpdateResumeRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await resumeService.UpdateResumeAsync(userId, id, request);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Resume);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteResume(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await resumeService.DeleteResumeAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
    
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> DownloadResume(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await resumeService.DownloadResumeAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return PhysicalFile(result.FilePath!, result.ContentType!, result.DownloadFileName!);
    }
}
