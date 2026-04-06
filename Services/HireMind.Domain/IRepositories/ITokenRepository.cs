namespace HireMind.Domain.IRepositories;

public interface ITokenRepository : IRepository<RefreshToken>
{
    Task AddToken(RefreshToken token, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByHash(string hash, CancellationToken cancellationToken);
    Task UpdateToken(RefreshToken token, CancellationToken cancellationToken);
}
