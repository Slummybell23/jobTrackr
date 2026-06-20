namespace CachesJobTrackerApi.Controllers.FollowUpReminderController.Models;

public class CreateFollowUpReminderRq
{
    public int JobId { get; set; }

    public required string FollowUpReminderTitle { get; set; }

    public string? FollowUpReminderNotes { get; set; }

    public DateTime FollowUpReminderDate { get; set; }
}
