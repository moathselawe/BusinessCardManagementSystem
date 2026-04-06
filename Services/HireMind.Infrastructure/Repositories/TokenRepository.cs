using HireMind.Application.Interfaces;
using HireMind.Domain.Entities.Security;
using HireMind.Domain.SeedWork;

namespace HireMind.Infrastructure.Repositories;

public class TokenRepository : BaseRepository<RefreshToken>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task AddToken(RefreshToken token, CancellationToken cancellationToken)
    {
        Add(token);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<RefreshToken?> GetByHash(string hash, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<RefreshToken>()
            .FirstOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);
    }

    public async Task UpdateToken(RefreshToken token, CancellationToken cancellationToken)
    {
        _dbContext.Set<RefreshToken>().Update(token);
    }
}

