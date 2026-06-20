using CachesJobTrackerApi.Controllers.FollowUpReminderController.Models;
using CachesJobTrackerApi.Database;
using CachesJobTrackerApi.Database.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace CachesJobTrackerApi.Services.FollowUpReminderService;

public class FollowUpReminderService(AppDbContext dbContext)
{
    public async Task<CreateFollowUpReminderResult> CreateFollowUpReminderAsync(string userId, CreateFollowUpReminderRq request)
    {
        // The reminder must attach to a job the user actually owns.
        var jobIsOwned = await dbContext.Jobs
            .AnyAsync(job => job.JobId == request.JobId && job.UserId == userId);
        if (!jobIsOwned)
            return CreateFollowUpReminderResult.Fail("Job application not found.");

        var reminderEntity = new FollowUpReminder
        {
            UserId = userId,
            JobId = request.JobId,
            FollowUpReminderTitle = request.FollowUpReminderTitle,
            FollowUpReminderNotes = request.FollowUpReminderNotes,
            FollowUpReminderDate = DateTime.SpecifyKind(request.FollowUpReminderDate, DateTimeKind.Utc),
            IsCompleted = false
        };

        await dbContext.FollowUpReminders.AddAsync(reminderEntity);
        await dbContext.SaveChangesAsync();

        return CreateFollowUpReminderResult.Ok(MapToClean(reminderEntity));
    }

    public async Task<ListFollowUpReminderResult> ListFollowUpRemindersAsync(string userId)
    {
        var reminderEntities = await dbContext.FollowUpReminders
            .Where(reminder => reminder.UserId == userId)
            .ToListAsync();

        return ListFollowUpReminderResult.Ok(reminderEntities.Select(MapToClean).ToList());
    }

    public async Task<UpdateFollowUpReminderResult> CompleteFollowUpReminderAsync(string userId, int reminderId)
    {
        var reminderEntity = await dbContext.FollowUpReminders
            .FirstOrDefaultAsync(reminder => reminder.FollowUpReminderId == reminderId && reminder.UserId == userId);

        if (reminderEntity is null)
            return UpdateFollowUpReminderResult.Fail("Reminder not found.");

        reminderEntity.IsCompleted = true;
        await dbContext.SaveChangesAsync();

        return UpdateFollowUpReminderResult.Ok(MapToClean(reminderEntity));
    }

    public async Task<UpdateFollowUpReminderResult> UpdateFollowUpReminderAsync(string userId, int reminderId, UpdateFollowUpReminderRq request)
    {
        var reminderEntity = await dbContext.FollowUpReminders
            .FirstOrDefaultAsync(reminder => reminder.FollowUpReminderId == reminderId && reminder.UserId == userId);

        if (reminderEntity is null)
            return UpdateFollowUpReminderResult.Fail("Reminder not found.");

        reminderEntity.FollowUpReminderTitle = request.FollowUpReminderTitle;
        reminderEntity.FollowUpReminderNotes = request.FollowUpReminderNotes;
        reminderEntity.FollowUpReminderDate = DateTime.SpecifyKind(request.FollowUpReminderDate, DateTimeKind.Utc);

        await dbContext.SaveChangesAsync();

        return UpdateFollowUpReminderResult.Ok(MapToClean(reminderEntity));
    }

    public async Task<DeleteFollowUpReminderResult> DeleteFollowUpReminderAsync(string userId, int reminderId)
    {
        var reminderEntity = await dbContext.FollowUpReminders
            .FirstOrDefaultAsync(reminder => reminder.FollowUpReminderId == reminderId && reminder.UserId == userId);

        if (reminderEntity is null)
            return DeleteFollowUpReminderResult.Fail("Reminder not found.");

        dbContext.FollowUpReminders.Remove(reminderEntity);
        await dbContext.SaveChangesAsync();

        return DeleteFollowUpReminderResult.Ok();
    }

    private static FollowUpReminderClean MapToClean(FollowUpReminder reminder) => new()
    {
        FollowUpReminderId = reminder.FollowUpReminderId,
        JobId = reminder.JobId,
        FollowUpReminderTitle = reminder.FollowUpReminderTitle,
        FollowUpReminderNotes = reminder.FollowUpReminderNotes,
        FollowUpReminderDate = reminder.FollowUpReminderDate,
        IsCompleted = reminder.IsCompleted
    };
}
