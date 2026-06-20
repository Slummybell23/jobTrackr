using Microsoft.AspNetCore.Identity;

namespace CachesJobTrackerApi.Database.EntityFrameworkModels;

public class User : IdentityUser
{
    public string? UserDisplayName { get; set; }

    // Navigation properties
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}
