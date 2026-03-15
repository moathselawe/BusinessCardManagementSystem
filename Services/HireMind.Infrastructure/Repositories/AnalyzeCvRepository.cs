namespace HireMind.Infrastructure.Repositories;

public class AnalyzeCvRepository : BaseRepository<AnalyzeCv>, IAnalyzeCvRepository
{
    public AnalyzeCvRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<int> AddAsync(AnalyzeCv cv, CancellationToken cancellationToken)
    {
        Add(cv);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await Task.FromResult(cv.Id);
    }

    //public async Task<AnalyzeCv?> GetByIdAsync(int id, CancellationToken cancellationToken)
    //{
    //    return await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
    //}
    public async Task<AnalyzeCv?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<AnalyzeCv>()
            .Include(x => x.Job) 
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null) return false;

        Delete(entity);
        return true;
    }

    public async Task<bool> UpdateAsync(AnalyzeCv analyzeCv, bool IsActivationOnly = false, CancellationToken cancellationToken = default)
    {
        var existing = await GetByIdQuery(analyzeCv.Id).FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
            return false;

        if (IsActivationOnly)
        {
            var jobWithUpdatedActivation = AnalyzeCv.Update(
            id: existing.Id,
            jobId: existing.JobId,
            cvFilePath: existing.CvFilePath,
            cvText: existing.CvText,
            aiScore: existing.AiScore,
            extractedAnswersJson: existing.ExtractedAnswersJson,
            isActive: analyzeCv.IsActive,
            emailAddress: existing.EmailAddress
            );

            Update(jobWithUpdatedActivation);
        }

        else
        {
            Update(analyzeCv);
        }

        return true;
    }

    public async Task<AnalyzeCv?> GetAnalyzedCvAsync(string emailAddress, int jobId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<AnalyzeCv>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmailAddress == emailAddress && x.JobId == jobId, cancellationToken);
    }
}
