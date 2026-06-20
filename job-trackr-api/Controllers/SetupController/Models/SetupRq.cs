namespace CachesJobTrackerApi.Controllers.SetupController.Models;

public class SetupRq
{
    // Database connection
    public required string Host { get; set; }
    public string Port { get; set; } = "5432";
    public required string Database { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    // First admin user
    public required string AdminEmail { get; set; }
    public required string AdminPassword { get; set; }
    public string? AdminDisplayName { get; set; }
}
