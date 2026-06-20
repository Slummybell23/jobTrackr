namespace CachesJobTrackerApi.Controllers.JobController.Models;

public class CreateJobApplicationRs
{
    public required int JobId { get; init; }

    public required string JobName { get; init; }

    public required string JobCompanyName { get; init; }

    public string? JobDescription { get; init; }

    public string? JobLink { get; init; }

    public string? JobNotes { get; init; }

    public string? JobApplicationStatus { get; init; }

    public string? JobLocation { get; init; }

    public DateOnly? JobAppliedDate { get; init; }

    public int? ResumeId { get; init; }

    public int? JobContactId { get; init; }
}
