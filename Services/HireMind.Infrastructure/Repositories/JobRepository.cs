using HireMind.Domain.Dtos.SharedDtos;
using HireMind.Domain.Entities.HireMind;

namespace HireMind.Infrastructure.Repositories;

public class JobRepository : BaseRepository<Job>, IJobRepository
{
    public JobRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public Task<int> AddAsync(Job job, CancellationToken cancellationToken)
    {
        Add(job);
        return Task.FromResult(job.Id);
    }

    public async Task<Job?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await GetByIdQuery(id)
            .Include(x => x.Location)
            .Include(x => x.JobType)
            .Include(x => x.WorkPlace)
            .Include(x => x.ContractType)
            .Include(x => x.OrganizationType)
            .Include(x => x.IndustrySector)
            .Include(x => x.HiringStages.OrderBy(s => s.StageOrder))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Job job, bool ActivationOnly = false, CancellationToken cancellationToken = default)
    {
        var existing = await GetByIdQuery(job.Id).FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
            return false;

        if (ActivationOnly)
        {
            var jobWithUpdatedActivation = Job.Update(
            id: existing.Id,
            title: existing.Title,
            description: existing.Description,
            locationId: existing.LocationId,
            workPlaceId: existing.WorkPlaceId,
            contractTypeId: existing.ContractTypeId,
            organizationTypeId: existing.OrganizationTypeId,
            industrySectorId: existing.IndustrySectorId,
            jobTypeId: existing.JobTypeId,
            companyId: existing.CompanyId,
            startDate: existing.StartDate,
            endDate: existing.EndDate,
            isActive: job.IsActive,
            questions: existing.Questions?.ConvertAll(q => new JobQuestion
            {
                QuestionText = q.QuestionText,
                QuestionTypeId = q.QuestionTypeId,
                IsRequired = q.IsRequired,
                AvailableAnswers = q.AvailableAnswers?
                .Select(a => new AnswerOption
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsPreferredAnswer = a.IsPreferredAnswer
                })
                .ToList() ?? new List<AnswerOption>(),
                Score = q.Score
            })
            );


            Update(jobWithUpdatedActivation);

        }
        else
        {
            // Update the whole entity
            Update(job);
        }

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null) return false;

        Delete(entity);
        return true;
    }

    public async Task<SearchFiltersRsDto<GetJobResponseDto>> SearchAsync(
    SearchFiltersRqDto filters,
    CancellationToken cancellationToken)
    {
        filters ??= new SearchFiltersRqDto(null);

        int pageNumber = filters.PageNumber <= 0 ? 1 : filters.PageNumber;
        int pageSize = filters.PageSize <= 0 ? 5 : filters.PageSize;
        string sortBy = string.IsNullOrWhiteSpace(filters.SortBy) ? "CreatedDate" : filters.SortBy;
        string orderBy = string.IsNullOrWhiteSpace(filters.OrderBy) ? "desc" : filters.OrderBy;

        IQueryable<Job> query = GetQuery()
            .Include(x => x.Location)
            .Include(x => x.JobType)
            .Include(x => x.WorkPlace)
            .Include(x => x.ContractType)
            .Include(x => x.OrganizationType)
            .Include(x => x.IndustrySector);

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            string term = filters.SearchTerm.Trim();

            query = query.Where(job =>
                job.Title.Contains(term) ||
                job.Description.Contains(term)
            );
        }

        if (filters.DateSearch.HasValue)
        {
            var date = filters.DateSearch.Value.Date;

            query = query.Where(job =>
                job.CreatedDate.Date == date
            );
        }

        query = (sortBy, orderBy.ToLower()) switch
        {
            ("CreatedDate", "desc") => query.OrderByDescending(x => x.CreatedDate),
            ("CreatedDate", "asc") => query.OrderBy(x => x.CreatedDate),

            ("Title", "desc") => query.OrderByDescending(x => x.Title),
            ("Title", "asc") => query.OrderBy(x => x.Title),

            ("StartDate", "desc") => query.OrderByDescending(x => x.StartDate),
            ("StartDate", "asc") => query.OrderBy(x => x.StartDate),

            _ => query.OrderByDescending(x => x.CreatedDate)
        };

        int totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(job => new GetJobResponseDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,

                LocationId = job.LocationId,
                LocationName = job.Location.CategoryName,

                JobTypeId = job.JobTypeId,
                JobTypeName = job.JobType.CategoryName,

                CompanyId = job.CompanyId,

                StartDate = job.StartDate,
                EndDate = job.EndDate,
                IsActive = job.IsActive
            })
            .ToListAsync(cancellationToken);

        return new SearchFiltersRsDto<GetJobResponseDto>(data, totalRecords);
    }
}
