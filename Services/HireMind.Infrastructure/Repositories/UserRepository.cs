using HireMind.Domain.Dtos.Security;
using HireMind.Domain.Entities.BCMS;
using HireMind.Domain.Entities.HireMind;

namespace HireMind.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly IPasswordHasher _hasher;
    public UserRepository(ApplicationDbContext dbContext, IPasswordHasher hasher)
        : base(dbContext)
    {
        _hasher = hasher;
    }
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .Include(x => x.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
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
    public async Task<User?> GetUserByEmailWithTokens(string email, CancellationToken cancellationToken)
    {
        return await GetQuery()
            .Where(u => u.Email == email)
            .Include(x => x.RefreshTokens)
            .Include(x => x.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(cancellationToken);
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
        var user = await _dbContext.Set<User>()
        .Include(u => u.UserRoles)
        .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return user;
    }
    public async Task<User> GetUserForRefreshById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .Where(u => u.Id == id)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<SearchFiltersRsDto<GetUserResponseDto>> SearchAsync(SearchFiltersRqDto filters,CancellationToken cancellationToken)
    {
        filters ??= new SearchFiltersRqDto(null);

        int pageNumber = filters.PageNumber <= 0 ? 1 : filters.PageNumber;
        int pageSize = filters.PageSize <= 0 ? 5 : filters.PageSize;
        string sortBy = string.IsNullOrWhiteSpace(filters.SortBy) ? "CreatedDate" : filters.SortBy;
        string orderBy = string.IsNullOrWhiteSpace(filters.OrderBy) ? "desc" : filters.OrderBy;

        IQueryable<User> query = GetQuery()
            .Include(x => x.UserRoles)
            .ThenInclude(ur => ur.Role);

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            string term = filters.SearchTerm.Trim();

            query = query.Where(user =>
                user.NameEnglish.Contains(term) ||
                user.NameArabic.Contains(term) ||
                user.Email.Contains(term) ||
                user.Mobile.Contains(term)
            );
        }

        if (filters.DateSearch.HasValue)
        {
            var date = filters.DateSearch.Value.Date;

            query = query.Where(user =>
                user.CreatedDate == date
            );
        }


        query = (sortBy, orderBy.ToLower()) switch
        {
            ("CreatedDate", "desc") => query.OrderByDescending(x => x.CreatedDate),
            ("CreatedDate", "asc") => query.OrderBy(x => x.CreatedDate),

            ("NameEnglish", "desc") => query.OrderByDescending(x => x.NameEnglish),
            ("NameEnglish", "asc") => query.OrderBy(x => x.NameEnglish),

            ("Email", "desc") => query.OrderByDescending(x => x.Email),
            ("Email", "asc") => query.OrderBy(x => x.Email),

            _ => query.OrderByDescending(x => x.CreatedDate)
        };

        int totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(user => new GetUserResponseDto
            {
                Id = user.Id,

                NameArabic = user.NameArabic,
                NameEnglish = user.NameEnglish,

                Email = user.Email,
                Mobile = user.Mobile,

                Address = user.Address,

                Gender = user.Gender,

                IsActive = user.IsActive,
                IsLocked = user.IsLocked,

                FailedLoginAttempts = user.FailedLoginAttempts,

                LockedDate = user.LockedDate,

                RoleIds = user.UserRoles
                    .Select(r => r.Role.Name)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return new SearchFiltersRsDto<GetUserResponseDto>(data, totalRecords);
    }
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdQuery(id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null) return false;

        Delete(entity);
        return true;
    }
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetQuery().ToListAsync(cancellationToken);
    }

    //public async Task RemoveUserRolesAsync(Guid userId, CancellationToken cancellationToken)
    //{
    //    var existingRoles = await _dbContext.Set<UserRole>()
    //        .Where(ur => ur.UserId == userId)
    //        .ToListAsync(cancellationToken);

    //    if (existingRoles.Count > 0)
    //        _dbContext.Set<UserRole>().RemoveRange(existingRoles);
    //}
}
