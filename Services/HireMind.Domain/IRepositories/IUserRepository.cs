using HireMind.Domain.Entities.HireMind;

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
    Task<User> GetUserById(Guid id, CancellationToken cancellationToken);
    //Task<User> GetUserByIdentifier(string identifier, CancellationToken cancellationToken);
    //Task Delete(string id, CancellationToken cancellationToken);
    //Task<List<User>> GetAll(CancellationToken cancellationToken);
    //Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
    //Task<User> GetUserById(string id, CancellationToken cancellationToken);
}
