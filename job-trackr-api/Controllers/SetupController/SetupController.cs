using System.Text.Json;
using CachesJobTrackerApi.Controllers.SetupController.Models;
using CachesJobTrackerApi.Database;
using CachesJobTrackerApi.Database.EntityFrameworkModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CachesJobTrackerApi.Controllers.SetupController;

// First-run installer. Anonymous by design (no DB/users exist yet), but every action refuses
// to run once the app is already configured, so it can't be abused after setup.
[ApiController]
[Route("setup")]
[AllowAnonymous]
public class SetupController(IConfiguration configuration, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        return Ok(new { isConfigured = await IsConfiguredAsync() });
    }

    [HttpPost]
    public async Task<IActionResult> RunSetup([FromBody] SetupRq request)
    {
        if (await IsConfiguredAsync())
            return Conflict("The application is already set up.");

        var connectionString =
            $"Host={request.Host};Port={request.Port};Database={request.Database};Username={request.Username};Password={request.Password}";

        // An isolated provider pointed at the supplied connection, so we can migrate + create the
        // first user against it directly (the app's own DI may still be on the placeholder connection).
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddIdentityCore<User>().AddEntityFrameworkStores<AppDbContext>();
        await using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        try
        {
            // Creates the database if needed and applies all migrations. Doubles as the connection test.
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            return BadRequest($"Could not connect to or migrate the database: {ex.Message}");
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        if (!await userManager.Users.AnyAsync())
        {
            var admin = new User
            {
                UserName = request.AdminEmail,
                Email = request.AdminEmail,
                EmailConfirmed = true,
                UserDisplayName = request.AdminDisplayName,
            };
            var result = await userManager.CreateAsync(admin, request.AdminPassword);
            if (!result.Succeeded)
                return BadRequest(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        // Persist the connection so the running app (via reload) and future restarts use it.
        await WriteConnectionStringAsync(connectionString);

        return Ok();
    }

    private async Task<bool> IsConfiguredAsync()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            return false;

        try
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options;
            await using var context = new AppDbContext(options);
            // Configured once the DB is reachable, migrated, and has at least one user.
            return await context.Database.CanConnectAsync() && await context.Users.AnyAsync();
        }
        catch
        {
            return false;
        }
    }

    private async Task WriteConnectionStringAsync(string connectionString)
    {
        var path = configuration["Setup:ConfigPath"]
            ?? Path.Combine(environment.ContentRootPath, "App_Data", "setup.json");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var payload = new { ConnectionStrings = new { DefaultConnection = connectionString } };
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
        await System.IO.File.WriteAllTextAsync(path, json);
    }
}
