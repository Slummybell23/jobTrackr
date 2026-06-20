namespace CachesJobTrackerApi.Controllers.ContactController.Models;

public class CreateContactRq
{
    public required string ContactFN { get; set; }

    public required string ContactLN { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactCompany { get; set; }

    public string? ContactPhoneNumber { get; set; }

    public string? ContactNotes { get; set; }
}
