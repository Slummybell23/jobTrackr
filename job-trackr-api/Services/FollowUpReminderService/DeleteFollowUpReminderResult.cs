namespace CachesJobTrackerApi.Services.FollowUpReminderService;

public record DeleteFollowUpReminderResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public static DeleteFollowUpReminderResult Fail(string error) => new() { Success = false, Error = error };

    public static DeleteFollowUpReminderResult Ok() => new() { Success = true };
}
