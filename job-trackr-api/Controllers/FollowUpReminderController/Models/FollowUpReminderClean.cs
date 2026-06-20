namespace CachesJobTrackerApi.Controllers.FollowUpReminderController.Models;

public class FollowUpReminderClean
{
    public int FollowUpReminderId { get; set; }

    public int JobId { get; set; }

    public required string FollowUpReminderTitle { get; set; }

    public string? FollowUpReminderNotes { get; set; }

    public DateTime FollowUpReminderDate { get; set; }

    public bool IsCompleted { get; set; }
}
