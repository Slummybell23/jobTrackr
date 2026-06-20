using CachesJobTrackerApi.Database.EntityFrameworkModels;

namespace CachesJobTrackerApi.Services.JobService;

public record CreateJobResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public Job? Job { get; init; }

    public static CreateJobResult Fail(string error) => new() { Success = false, Error = error };

    public static CreateJobResult Ok(Job job) => new() { Success = true, Job = job };
}
