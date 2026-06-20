namespace CachesJobTrackerApi.Services.JobService;

public record DeleteJobResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public static DeleteJobResult Fail(string error) => new() { Success = false, Error = error };

    public static DeleteJobResult Ok() => new() { Success = true };
}
