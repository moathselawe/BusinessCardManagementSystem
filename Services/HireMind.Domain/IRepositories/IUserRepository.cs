namespace HireMind.Domain.IRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> EmailExists(string email, CancellationToken cancellationToken);
    Task<Guid> CreateUser(User user, CancellationToken cancellationToken);
    Task<bool> ModifyUser(User user, CancellationToken cancellationToken);
    Task<User> GetUserByPlainToken(string plainToken, CancellationToken cancellationToken);
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
    Task<User> GetUserByEmailWithTokens(string email, CancellationToken cancellationToken);
    Task<User?> GetUserByRefreshTokenHash(string tokenHash, CancellationToken cancellationToken);
    Task<User> GetUserById(Guid id, CancellationToken cancellationToken = default);
    Task<SearchFiltersRsDto<GetUserResponseDto>> SearchAsync(SearchFiltersRqDto filters, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
}
