using CachesJobTrackerApi.Controllers.JobController.Models;

namespace CachesJobTrackerApi.Services.JobService;

public record ListJobResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public List<JobClean>? Jobs { get; init; }

    public static ListJobResult Fail(string error) => new() { Success = false, Error = error };

    public static ListJobResult Ok(List<JobClean> jobs) => new() { Success = true, Jobs = jobs };
}
