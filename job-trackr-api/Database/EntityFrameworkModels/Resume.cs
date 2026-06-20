using System.ComponentModel.DataAnnotations.Schema;

namespace CachesJobTrackerApi.Database.EntityFrameworkModels;

public class Resume
{
    public int ResumeId { get; set; }

    public string UserId { get; set; }

    public required string ResumeName { get; set; }

    public required string ResumeFilePath { get; set; }

    public string? ResumeNotes { get; set; }

    public DateTime ResumeCreatedDate { get; set; }

    public List<string> ResumeLabels { get; set; } = new();

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}

public class ResumeClean
{
    public int ResumeId { get; set; }
    
    public string ResumeName { get; set; }
    
    public string? ResumeNotes { get; set; }

    public DateTime ResumeCreatedDate { get; set; }

    public List<string> ResumeLabels { get; set; } = new();

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}