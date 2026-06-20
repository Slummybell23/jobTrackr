using CachesJobTrackerApi.Database.EntityFrameworkModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CachesJobTrackerApi.Database;

public class AppDbContext: IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<FollowUpReminder> FollowUpReminders { get; set; }
}