namespace CachesJobTrackerApi.Controllers.ResumeController.Models;

public class CreateResumeRs
{
    public required int ResumeId { get; init; }

    public required string ResumeName { get; init; }

    public required string ResumeFilePath { get; init; }

    public required DateTime ResumeCreatedDate { get; init; }
}
