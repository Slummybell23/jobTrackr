using System.Security.Claims;
using CachesJobTrackerApi.Controllers.JobController.Models;
using CachesJobTrackerApi.Services.JobService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CachesJobTrackerApi.Controllers.JobController;

[ApiController]
[Route("[controller]")]
[Authorize]
public class JobController(JobService jobService) : ControllerBase
{
    [HttpPost("createJob")]
    [Authorize]
    public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationRq request)
    {
        // The authenticated user's id comes from the bearer token's NameIdentifier claim.
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var createdJobEntity = await jobService.CreateJobApplicationAsync(userId, request);
        
        if (!createdJobEntity.Success)
            return BadRequest(createdJobEntity);
        
        var jobCreatedResponse = new CreateJobApplicationRs()
        {
            JobId = createdJobEntity.Job!.JobId,
            JobCompanyName = createdJobEntity.Job.JobCompanyName,
            JobName = createdJobEntity.Job.JobName,
            JobApplicationStatus = createdJobEntity.Job.JobApplicationStatus,
            JobAppliedDate = createdJobEntity.Job.JobAppliedDate,
            JobContactId = createdJobEntity.Job.JobContactId,
            JobDescription = createdJobEntity.Job.JobDescription,
            JobLink = createdJobEntity.Job.JobLink,
            JobLocation = createdJobEntity.Job.JobLocation,
            JobNotes = createdJobEntity.Job.JobNotes,
        };
        
        return Ok(jobCreatedResponse);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListJobs()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await jobService.ListJobsAsync(userId);
        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Jobs);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetJob(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await jobService.GetJobAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Job);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobApplicationRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await jobService.UpdateJobAsync(userId, id, request);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Job);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateJobStatus(int id, [FromBody] UpdateJobStatusRq request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await jobService.UpdateJobStatusAsync(userId, id, request);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Job);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await jobService.DeleteJobAsync(userId, id);
        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
}
