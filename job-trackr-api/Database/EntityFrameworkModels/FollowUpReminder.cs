using System.ComponentModel.DataAnnotations.Schema;

namespace CachesJobTrackerApi.Database.EntityFrameworkModels;

public class FollowUpReminder
{
    public int FollowUpReminderId { get; set; }

    public string UserId { get; set; } = null!;

    public int JobId { get; set; }

    public required string FollowUpReminderTitle { get; set; }

    public string? FollowUpReminderNotes { get; set; }

    public DateTime FollowUpReminderDate { get; set; }

    public bool IsCompleted { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(JobId))]
    public Job Job { get; set; } = null!;
}
