using CachesJobTrackerApi.Controllers.JobController.Models;
using CachesJobTrackerApi.Database;
using CachesJobTrackerApi.Database.EntityFrameworkModels;
using Microsoft.EntityFrameworkCore;

namespace CachesJobTrackerApi.Services.JobService;

public class JobService(AppDbContext dbContext)
{
    public async Task<CreateJobResult> CreateJobApplicationAsync(string userId, CreateJobApplicationRq request)
    {
        var jobEntity = new Job()
        {
            UserId = userId,
            JobCompanyName =  request.JobCompanyName,
            JobName =  request.JobName,
            JobApplicationStatus = request.JobApplicationStatus,
            JobAppliedDate =  request.JobAppliedDate,
            JobContactId = request.JobContactId,
            JobDescription =  request.JobDescription,
            JobLink = request.JobLink,
            JobLocation = request.JobLocation,
            JobNotes =  request.JobNotes,
            ResumeId = request.ResumeId,
            JobSalary = request.JobSalary,
            JobWorkMode = request.JobWorkMode,
            JobSource = request.JobSource,
            JobTags = request.JobTags,
        };
        
        await dbContext.Jobs.AddAsync(jobEntity);
        await dbContext.SaveChangesAsync();

        return CreateJobResult.Ok(jobEntity);
    }

    public async Task<ListJobResult> ListJobsAsync(string userId)
    {
        var jobEntities = await dbContext.Jobs
            .Where(job => job.UserId == userId)
            .ToListAsync();

        var jobModels = jobEntities.Select(MapToClean).ToList();

        return ListJobResult.Ok(jobModels);
    }

    public async Task<GetJobResult> GetJobAsync(string userId, int jobId)
    {
        var jobEntity = await dbContext.Jobs
            .FirstOrDefaultAsync(job => job.JobId == jobId && job.UserId == userId);

        if (jobEntity is null)
            return GetJobResult.Fail("Job application not found.");

        return GetJobResult.Ok(MapToClean(jobEntity));
    }

    public async Task<UpdateJobResult> UpdateJobAsync(string userId, int jobId, UpdateJobApplicationRq request)
    {
        var jobEntity = await dbContext.Jobs
            .FirstOrDefaultAsync(job => job.JobId == jobId && job.UserId == userId);

        if (jobEntity is null)
            return UpdateJobResult.Fail("Job application not found.");

        jobEntity.JobName = request.JobName;
        jobEntity.JobCompanyName = request.JobCompanyName;
        jobEntity.JobDescription = request.JobDescription;
        jobEntity.JobLink = request.JobLink;
        jobEntity.JobNotes = request.JobNotes;
        jobEntity.JobApplicationStatus = request.JobApplicationStatus;
        jobEntity.JobLocation = request.JobLocation;
        jobEntity.JobAppliedDate = request.JobAppliedDate;
        jobEntity.ResumeId = request.ResumeId;
        jobEntity.JobContactId = request.JobContactId;
        jobEntity.JobSalary = request.JobSalary;
        jobEntity.JobWorkMode = request.JobWorkMode;
        jobEntity.JobSource = request.JobSource;
        jobEntity.JobTags = request.JobTags;

        await dbContext.SaveChangesAsync();

        return UpdateJobResult.Ok(MapToClean(jobEntity));
    }

    public async Task<UpdateJobResult> UpdateJobStatusAsync(string userId, int jobId, UpdateJobStatusRq request)
    {
        var jobEntity = await dbContext.Jobs
            .FirstOrDefaultAsync(job => job.JobId == jobId && job.UserId == userId);

        if (jobEntity is null)
            return UpdateJobResult.Fail("Job application not found.");

        jobEntity.JobApplicationStatus = request.JobApplicationStatus;

        await dbContext.SaveChangesAsync();

        return UpdateJobResult.Ok(MapToClean(jobEntity));
    }

    public async Task<DeleteJobResult> DeleteJobAsync(string userId, int jobId)
    {
        var jobEntity = await dbContext.Jobs
            .FirstOrDefaultAsync(job => job.JobId == jobId && job.UserId == userId);

        if (jobEntity is null)
            return DeleteJobResult.Fail("Job application not found.");

        dbContext.Jobs.Remove(jobEntity);
        await dbContext.SaveChangesAsync();

        return DeleteJobResult.Ok();
    }

    private static JobClean MapToClean(Job job) => new()
    {
        JobId = job.JobId,
        JobName = job.JobName,
        JobCompanyName = job.JobCompanyName,
        JobDescription = job.JobDescription,
        JobLink = job.JobLink,
        JobNotes = job.JobNotes,
        JobApplicationStatus = job.JobApplicationStatus,
        JobLocation = job.JobLocation,
        JobAppliedDate = job.JobAppliedDate,
        ResumeId = job.ResumeId,
        JobContactId = job.JobContactId,
        JobSalary = job.JobSalary,
        JobWorkMode = job.JobWorkMode,
        JobSource = job.JobSource,
        JobTags = job.JobTags
    };
}
