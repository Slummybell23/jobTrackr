using CachesJobTrackerApi.Controllers.JobController.Models;

namespace CachesJobTrackerApi.Services.JobService;

public record GetJobResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public JobClean? Job { get; init; }

    public static GetJobResult Fail(string error) => new() { Success = false, Error = error };

    public static GetJobResult Ok(JobClean job) => new() { Success = true, Job = job };
}
