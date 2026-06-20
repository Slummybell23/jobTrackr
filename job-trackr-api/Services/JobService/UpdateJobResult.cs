using CachesJobTrackerApi.Controllers.JobController.Models;

namespace CachesJobTrackerApi.Services.JobService;

public record UpdateJobResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public JobClean? Job { get; init; }

    public static UpdateJobResult Fail(string error) => new() { Success = false, Error = error };

    public static UpdateJobResult Ok(JobClean job) => new() { Success = true, Job = job };
}
