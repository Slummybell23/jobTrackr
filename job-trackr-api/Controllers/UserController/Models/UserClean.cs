namespace CachesJobTrackerApi.Controllers.UserController.Models;

public class UserClean
{
    public required string Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? UserDisplayName { get; set; }
}
