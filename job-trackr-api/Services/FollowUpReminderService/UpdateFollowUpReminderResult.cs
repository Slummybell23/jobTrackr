using CachesJobTrackerApi.Controllers.FollowUpReminderController.Models;

namespace CachesJobTrackerApi.Services.FollowUpReminderService;

public record UpdateFollowUpReminderResult
{
    public required bool Success { get; init; }

    public string? Error { get; init; }

    public FollowUpReminderClean? Reminder { get; init; }

    public static UpdateFollowUpReminderResult Fail(string error) => new() { Success = false, Error = error };

    public static UpdateFollowUpReminderResult Ok(FollowUpReminderClean reminder) => new() { Success = true, Reminder = reminder };
}
