namespace CachesJobTrackerApi.Controllers.FollowUpReminderController.Models;

public class UpdateFollowUpReminderRq
{
    public required string FollowUpReminderTitle { get; set; }

    public string? FollowUpReminderNotes { get; set; }

    public DateTime FollowUpReminderDate { get; set; }
}
