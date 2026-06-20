namespace CachesJobTrackerApi.Controllers.JobController.Models;

public class UpdateJobApplicationRq
{
    public required string JobName { get; set; }

    public required string JobCompanyName { get; set; }

    public string? JobDescription { get; set; }

    public string? JobLink { get; set; }

    public string? JobNotes { get; set; }

    public string? JobApplicationStatus { get; set; }

    public string? JobLocation { get; set; }

    public DateOnly? JobAppliedDate { get; set; }

    public int? ResumeId { get; set; }

    public int? JobContactId { get; set; }

    public string? JobSalary { get; set; }

    public string? JobWorkMode { get; set; }

    public string? JobSource { get; set; }

    public List<string> JobTags { get; set; } = new();
}
