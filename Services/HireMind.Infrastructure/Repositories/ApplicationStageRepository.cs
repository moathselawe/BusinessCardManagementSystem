using HireMind.Domain.Dtos.ApplicationStage;
using HireMind.Domain.Dtos.JobApplication;
using HireMind.Domain.Dtos.SharedDtos;
using HireMind.Domain.Entities.HireMind;
using HireMind.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace HireMind.Infrastructure.Repositories;

public class ApplicationStageRepository : BaseRepository<ApplicationStage>, IApplicationStageRepository
{
    public ApplicationStageRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<int> AddAsync(ApplicationStage applicationStage, CancellationToken cancellationToken)
    {
        Add(applicationStage);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(applicationStage.Id);
    }

    public async Task<int> UpdateBulkApplicationStagesStatusBulkAsync(
    List<int> jobApplicationIds,
    StageStatus newStatus,
    CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ApplicationStage>()
            .Where(x => jobApplicationIds.Contains(x.JobApplicationId))
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Status, newStatus),
                cancellationToken);
    }

    public async Task AddRangeAsync(List<ApplicationStage> stages, CancellationToken cancellationToken)
    {
        await _dbContext.Set<ApplicationStage>().AddRangeAsync(stages, cancellationToken);
    }

    //public async Task<List<JobApplicationDto>> SearchJobApplicationsAsync(
    //SearchJobApplicationsRequestDto request,
    //CancellationToken cancellationToken)
    //{
    //    var hasSearch = !string.IsNullOrWhiteSpace(request.SearchInput);
    //    var hasStage = request.StageId.HasValue;
    //    var hasStageStatus = request.StageStatusId.HasValue;

    //    // Step 1: base query
    //    var baseQuery = _dbContext.Set<ApplicationStage>()
    //        .Where(x => x.JobApplication.JobId == request.JobId);

    //    // Step 2: apply stage filter if provided
    //    if (hasStage)
    //    {
    //        baseQuery = baseQuery.Where(x => x.HiringStageId == request.StageId);
    //    }

    //    // Step 3: apply stage status filter if provided
    //    if (hasStageStatus)
    //    {
    //        baseQuery = baseQuery.Where(x => (int)x.Status == request.StageStatusId.Value);
    //    }
    //    else if (!hasStage)
    //    {
    //        // If no stage or status filter, get last stage per application
    //        var lastStageIds = await baseQuery
    //            .GroupBy(x => x.JobApplicationId)
    //            .Select(g => g
    //                .OrderByDescending(x => x.HiringStage.StageOrder)
    //                .Select(x => x.Id)
    //                .FirstOrDefault()
    //            )
    //            .ToListAsync(cancellationToken);

    //        baseQuery = baseQuery.Where(x => lastStageIds.Contains(x.Id));
    //    }

    //    // Step 4: apply search filter
    //    if (hasSearch)
    //    {
    //        var search = request.SearchInput!.ToLower();
    //        baseQuery = baseQuery.Where(x =>
    //            x.JobApplication.PersonalInfo.FullName.ToLower().Contains(search) ||
    //            x.JobApplication.PersonalInfo.EmailAddress.ToLower().Contains(search) ||
    //            x.JobApplication.PersonalInfo.MobileNumber.Contains(search)
    //        );
    //    }

    //    // Step 5: include necessary navigation properties
    //    var query = baseQuery
    //        .Include(x => x.JobApplication)
    //            .ThenInclude(j => j.PersonalInfo)
    //                .ThenInclude(p => p.CountryCode)
    //        .Include(x => x.JobApplication.Job)
    //        .Include(x => x.HiringStage);

    //    // Step 6: project to DTO
    //    var result = await query
    //        .Select(x => new JobApplicationDto
    //        {
    //            Id = x.JobApplication.Id,
    //            FullName = x.JobApplication.PersonalInfo.FullName,
    //            Email = x.JobApplication.PersonalInfo.EmailAddress,
    //            CountryCodeId = x.JobApplication.PersonalInfo.CountryCodeId,
    //            CountryCode = x.JobApplication.PersonalInfo.CountryCode.CategoryName,
    //            MobileNumber = x.JobApplication.PersonalInfo.MobileNumber,
    //            SystemScore = x.JobApplication.SystemScore,
    //            TotalScore = x.JobApplication.TotalScore,
    //            JobTitle = x.JobApplication.Job.Title,

    //            CurrentStageId = x.HiringStage.Id,
    //            CurrentStageName = x.HiringStage.Name,
    //            CurrentStageOrder = x.HiringStage.StageOrder,

    //            ApplicationStageId = x.Id,
    //            HiringStageId = x.HiringStageId,
    //            Status = x.Status.ToString()
    //        })
    //        .OrderByDescending(x => x.TotalScore)
    //        .ToListAsync(cancellationToken);

    //    return result;
    //}

    public async Task<List<JobApplicationDto>> SearchJobApplicationsAsync(
    SearchJobApplicationsRequestDto request,
    CancellationToken cancellationToken)
    {
        var hasSearch = !string.IsNullOrWhiteSpace(request.SearchInput);
        var hasStage = request.StageId.HasValue;
        var hasStageStatus = request.StageStatusId.HasValue;

        // Step 1: base query
        var baseQuery = _dbContext.Set<ApplicationStage>()
            .Where(x => x.JobApplication.JobId == request.JobId);

        // Step 2: apply stage filter if provided
        if (hasStage)
        {
            baseQuery = baseQuery.Where(x => x.HiringStageId == request.StageId);
        }

        // Step 3: apply stage status filter if provided
        if (hasStageStatus)
        {
            baseQuery = baseQuery.Where(x => (int)x.Status == request.StageStatusId.Value);
        }
        else if (!hasStage)
        {
            // If no stage or status filter, get last stage per application
            var lastStageIds = await baseQuery
                .GroupBy(x => x.JobApplicationId)
                .Select(g => g
                    .OrderByDescending(x => x.HiringStage.StageOrder)
                    .Select(x => x.Id)
                    .FirstOrDefault()
                )
                .ToListAsync(cancellationToken);

            baseQuery = baseQuery.Where(x => lastStageIds.Contains(x.Id));
        }

        // Step 4: apply search filter
        if (hasSearch)
        {
            var search = request.SearchInput!.ToLower();
            baseQuery = baseQuery.Where(x =>
                x.JobApplication.PersonalInfo.FullName.ToLower().Contains(search) ||
                x.JobApplication.PersonalInfo.EmailAddress.ToLower().Contains(search) ||
                x.JobApplication.PersonalInfo.MobileNumber.Contains(search)
            );
        }

        // Step 5: include necessary navigation properties
        var query = baseQuery
            .Include(x => x.JobApplication)
                .ThenInclude(j => j.PersonalInfo)
                    .ThenInclude(p => p.CountryCode)
            .Include(x => x.JobApplication.Job)
            .Include(x => x.HiringStage);

        // Step 6: project to DTO
        var result = await query
            .Select(x => new JobApplicationDto
            {
                Id = x.JobApplication.Id,
                FullName = x.JobApplication.PersonalInfo.FullName,
                Email = x.JobApplication.PersonalInfo.EmailAddress,
                CountryCodeId = x.JobApplication.PersonalInfo.CountryCodeId,
                CountryCode = x.JobApplication.PersonalInfo.CountryCode.CategoryName,
                MobileNumber = x.JobApplication.PersonalInfo.MobileNumber,
                SystemScore = x.JobApplication.SystemScore,
                TotalScore = x.JobApplication.TotalScore,
                JobTitle = x.JobApplication.Job.Title,

                CurrentStageId = x.HiringStage.Id,
                CurrentStageName = x.HiringStage.Name,
                CurrentStageOrder = x.HiringStage.StageOrder,

                ApplicationStageId = x.Id,
                HiringStageId = x.HiringStageId,
                Status = x.Status.ToString()
            })
            .OrderByDescending(x => x.TotalScore)  // always order by total score descending
            .ToListAsync(cancellationToken);

        // Step 7: apply short list limit if provided
        if (request.Limit.HasValue && request.Limit.Value > 0)
        {
            result = result.Take(request.Limit.Value).ToList();
        }

        return result;
    }

    //public async Task<List<JobApplicationDto>> SearchJobApplicationsAsync(
    //SearchJobApplicationsRequestDto request,
    //CancellationToken cancellationToken)
    //{
    //    var hasSearch = !string.IsNullOrWhiteSpace(request.SearchInput);
    //    var hasStage = request.StageId.HasValue;

    //    // Step 1: base query
    //    var baseQuery = _dbContext.Set<ApplicationStage>()
    //        .Where(x => x.JobApplication.JobId == request.JobId);

    //    // Step 2: get last stage IDs per application if no stage filter
    //    List<int> lastStageIds = new();
    //    if (!hasStage)
    //    {
    //        lastStageIds = await baseQuery
    //            .GroupBy(x => x.JobApplicationId)
    //            .Select(g => g
    //                .OrderByDescending(x => x.HiringStage.StageOrder)
    //                .Select(x => x.Id)
    //                .FirstOrDefault()
    //            )
    //            .ToListAsync(cancellationToken);

    //        baseQuery = baseQuery.Where(x => lastStageIds.Contains(x.Id));
    //    }
    //    else
    //    {
    //        // stage filter
    //        baseQuery = baseQuery.Where(x => x.HiringStageId == request.StageId);
    //    }

    //    // Step 3: apply search filter
    //    if (hasSearch)
    //    {
    //        var search = request.SearchInput!.ToLower();
    //        baseQuery = baseQuery.Where(x =>
    //            x.JobApplication.PersonalInfo.FullName.ToLower().Contains(search) ||
    //            x.JobApplication.PersonalInfo.EmailAddress.ToLower().Contains(search) ||
    //            x.JobApplication.PersonalInfo.MobileNumber.Contains(search)
    //        );
    //    }

    //    // Step 4: include necessary navigation properties
    //    var query = baseQuery
    //        .Include(x => x.JobApplication)
    //            .ThenInclude(j => j.PersonalInfo)
    //                .ThenInclude(p => p.CountryCode)
    //        .Include(x => x.JobApplication.Job)
    //        .Include(x => x.HiringStage);

    //    // Step 5: project to DTO
    //    var result = await query
    //        .Select(x => new JobApplicationDto
    //        {
    //            Id = x.JobApplication.Id,
    //            FullName = x.JobApplication.PersonalInfo.FullName,
    //            Email = x.JobApplication.PersonalInfo.EmailAddress,
    //            CountryCodeId = x.JobApplication.PersonalInfo.CountryCodeId,
    //            CountryCode = x.JobApplication.PersonalInfo.CountryCode.CategoryName,
    //            MobileNumber = x.JobApplication.PersonalInfo.MobileNumber,
    //            SystemScore = x.JobApplication.SystemScore,
    //            TotalScore = x.JobApplication.TotalScore,
    //            JobTitle = x.JobApplication.Job.Title,

    //            CurrentStageId = x.HiringStage.Id,
    //            CurrentStageName = x.HiringStage.Name,
    //            CurrentStageOrder = x.HiringStage.StageOrder,

    //            ApplicationStageId = x.Id,
    //            HiringStageId = x.HiringStageId,
    //            Status = x.Status.ToString()
    //        })
    //        .OrderByDescending(x => x.TotalScore)
    //        .ToListAsync(cancellationToken);

    //    return result;
    //}

}