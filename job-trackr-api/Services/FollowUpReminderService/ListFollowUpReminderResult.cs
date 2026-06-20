using CachesJobTrackerApi.Controllers.FollowUpReminderController.Models;

namespace CachesJobTrackerApi.Services.FollowUpReminderService;

public record ListFollowUpReminderResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public List<FollowUpReminderClean>? Reminders { get; init; }

    public static ListFollowUpReminderResult Fail(string error) => new() { Success = false, Error = error };

    public static ListFollowUpReminderResult Ok(List<FollowUpReminderClean> reminders) => new() { Success = true, Reminders = reminders };
}
