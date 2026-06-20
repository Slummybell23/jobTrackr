namespace CachesJobTrackerApi.Controllers.ResumeController.Models;

public class UpdateResumeRq
{
    public required string ResumeName { get; set; }

    public string? ResumeNotes { get; set; }

    public List<string> ResumeLabels { get; set; } = new();
}
