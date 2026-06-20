using System.ComponentModel.DataAnnotations.Schema;

namespace CachesJobTrackerApi.Database.EntityFrameworkModels;

public class Job
{
    public int JobId { get; set; }

    public string UserId { get; set; }

    public int? ResumeId { get; set; }

    public required string JobName { get; set; }

    public string? JobDescription { get; set; }

    public string? JobLink { get; set; }

    public int? JobContactId { get; set; }

    public string? JobNotes { get; set; }

    public string? JobApplicationStatus { get; set; }

    public required string JobCompanyName { get; set; }

    public string? JobLocation { get; set; }

    public DateOnly? JobAppliedDate { get; set; }

    public string? JobSalary { get; set; }

    public string? JobWorkMode { get; set; }

    public string? JobSource { get; set; }

    public List<string> JobTags { get; set; } = new();

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(ResumeId))]
    public Resume? Resume { get; set; }

    [ForeignKey(nameof(JobContactId))]
    public Contact? JobContact { get; set; }
}
