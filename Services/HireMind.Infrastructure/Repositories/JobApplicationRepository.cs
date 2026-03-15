using HireMind.Domain.Dtos.JobApplication;
using Microsoft.EntityFrameworkCore;

namespace HireMind.Infrastructure.Repositories;

public class JobApplicationRepository : BaseRepository<JobApplication>, IJobApplicationRepository
{
    public JobApplicationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<int> AddAsync(JobApplication jobApplication, CancellationToken cancellationToken)
    {
        Add(jobApplication);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(jobApplication.Id);
    }

    public async Task<bool> UpdateAsync(JobApplication jobApplication, CancellationToken cancellationToken = default)
    {
        var existing = await GetQuery().FirstOrDefaultAsync(l => l.Id == jobApplication.Id, cancellationToken);

        if (existing == null)
            return false;

        Update(jobApplication);

        return true;
    }

    public async Task<bool> CheckApplicationByEmailAndJobId(string email, int jobId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<JobApplication>()
            .AnyAsync(x => x.PersonalInfo.EmailAddress == email && x.JobId == jobId, cancellationToken);
    }

    public async Task<List<JobApplicationDto>> GetAllJobApplicationsByJobIdAsync(
        int jobid,
        CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<JobApplication>()
            .Where(x => x.JobId == jobid)
            .Include(x => x.Job)
            .Include(x => x.CurrentStage)
                .ThenInclude(x => x.HiringStage)
            .Include(x => x.PersonalInfo.CountryCode) // include lookup for CountryCode
            .Select(x => new JobApplicationDto
            {
                Id = x.Id,
                FullName = x.PersonalInfo.FullName,
                Email = x.PersonalInfo.EmailAddress,
                CountryCodeId = x.PersonalInfo.CountryCodeId,
                CountryCode = x.PersonalInfo.CountryCode.CategoryName,
                MobileNumber = x.PersonalInfo.MobileNumber,
                SystemScore = x.SystemScore,
                TotalScore = x.TotalScore,
                JobTitle = x.Job.Title,
                CurrentStageName = x.CurrentStage != null ? x.CurrentStage.HiringStage.Name : string.Empty,
                CurrentStageOrder = x.CurrentStage != null ? x.CurrentStage.HiringStage.StageOrder : 0,
                ApplicationStageId = x.CurrentStage != null ? x.CurrentStage.Id : 0,
                HiringStageId = x.CurrentStage != null ? x.CurrentStage.HiringStageId : 0,
                Status = x.CurrentStage != null ? x.CurrentStage.Status.ToString() : string.Empty
            })
            .OrderByDescending(x => x.TotalScore)
            .ToListAsync(cancellationToken);

        return result;
    }
}
