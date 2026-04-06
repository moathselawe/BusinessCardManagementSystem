using DocumentFormat.OpenXml.Spreadsheet;
using HireMind.Application.Interfaces;
using HireMind.Domain.Entities.Security;
using HireMind.Domain.SeedWork;

namespace HireMind.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly IPasswordHasher _hasher;

    public UserRepository(ApplicationDbContext dbContext, IPasswordHasher hasher)
        : base(dbContext)
    {
        _hasher = hasher;
    }

    public async Task<bool> EmailExists(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .AnyAsync(x => x.Email == email, cancellationToken);
    }
    public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
    {
        Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
    public async Task<bool> ModifyUser(User user, CancellationToken cancellationToken)
    {
        var existing = await GetQuery().FirstOrDefaultAsync(l => l.Id == user.Id, cancellationToken);

        if (existing == null)
            return false;

        Update(user);

        return true;
    }

    public async Task<User?> GetUserByPlainToken(string plainToken, CancellationToken cancellationToken)
    {
        var users = await GetQuery()
            .Where(u => u.EmailVerificationToken != null)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            if (_hasher.Verify(plainToken, user.EmailVerificationToken))
                return user;
        }

        return null;
    }

    public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var user = await GetQuery()
                    .Where(u => u.Email == email)
                    .FirstOrDefaultAsync(cancellationToken);

        return user;
    }



    public async Task<User> GetUserByEmailWithTokens(string email, CancellationToken cancellationToken)
    {
        var user = await GetQuery()
                    .Where(u => u.Email == email)
                    .Include(u => u.RefreshTokens)
                    .FirstOrDefaultAsync(cancellationToken);

        return user;
    }
    public async Task<User?> GetUserByRefreshTokenHash(string tokenHash, CancellationToken cancellationToken)
    {
        return await GetQuery()
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(
                u => u.RefreshTokens.Any(t => t.TokenHash == tokenHash),
                cancellationToken);
    }
    public async Task<User> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetQuery()
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);

        return user;
    }

    //public async Task Delete(string id, CancellationToken cancellationToken)
    //{
    //    await _repository.DeleteAsync(id, cancellationToken);
    //}

    //public async Task<List<User>> GetAll(CancellationToken cancellationToken)
    //{
    //    return await _repository.GetAllAsync(cancellationToken);
    //}



    //public async Task<User> GetUserById(string id, CancellationToken cancellationToken)
    //{
    //    return await _repository.GetByIdAsync(id, cancellationToken);
    //}

    //public async Task<User> GetUserByIdentifier(string identifier, CancellationToken cancellationToken)
    //{
    //        return await _repository.Queryable
    //            .Where(u => u.Email == identifier
    //                     || u.Mobile == identifier
    //                     || u.Username == identifier)
    //            .FirstOrDefaultAsync(cancellationToken);
    //}
}
