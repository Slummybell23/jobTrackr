using System.ComponentModel.DataAnnotations.Schema;

namespace CachesJobTrackerApi.Database.EntityFrameworkModels;

public class Contact
{
    public int ContactId { get; set; }
    
    public string UserId { get; set; }

    public required string ContactFN { get; set; }

    public required string ContactLN { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactCompany { get; set; }

    public string? ContactPhoneNumber { get; set; }

    public string? ContactNotes { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
