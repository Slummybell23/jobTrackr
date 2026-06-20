using Microsoft.AspNetCore.Http;

namespace CachesJobTrackerApi.Controllers.ResumeController.Models;

public class CreateResumeRq
{
    public required string ResumeName { get; set; }

    public string? ResumeNotes { get; set; }

    public List<string> ResumeLabels { get; set; } = new();

    public required IFormFile File { get; set; }
}
