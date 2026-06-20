using CachesJobTrackerApi.Controllers.UserController.Models;
using CachesJobTrackerApi.Database;
using CachesJobTrackerApi.Database.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace CachesJobTrackerApi.Services.UserService;

public class UserService(AppDbContext dbContext)
{
    public async Task<GetUserResult> GetCurrentUserAsync(string userId)
    {
        var userEntity = await dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (userEntity is null)
            return GetUserResult.Fail("User not found.");

        return GetUserResult.Ok(MapToClean(userEntity));
    }

    public async Task<UpdateUserResult> UpdateCurrentUserAsync(string userId, UpdateUserRq request)
    {
        var userEntity = await dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (userEntity is null)
            return UpdateUserResult.Fail("User not found.");

        userEntity.UserDisplayName = request.UserDisplayName;
        await dbContext.SaveChangesAsync();

        return UpdateUserResult.Ok(MapToClean(userEntity));
    }

    private static UserClean MapToClean(User user) => new()
    {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        UserDisplayName = user.UserDisplayName
    };
}
