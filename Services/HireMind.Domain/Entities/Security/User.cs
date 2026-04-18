using HireMind.Domain.Entities.HireMind;
using HireMind.Domain.Enum;
using System.Net;
using System.Reflection;

namespace HireMind.Domain.Entities.Security;
public class User : Entity<Guid>
{
    public string NameArabic { get; private set; } = null!;
    public string NameEnglish { get; private set; } = null!;
    public string Mobile { get; private set; } = null!;
    public string? Address { get; private set; }
    public byte[]? ProfileImage { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? GoogleId { get; private set; }
    public bool IsActive { get; private set; } = false;
    public int FailedLoginAttempts { get; private set; } = 0;
    public bool IsLocked { get; private set; } = false;
    public DateTime? LockedDate { get; private set; }
    public Gender Gender { get; private set; }
    public List<RefreshToken> RefreshTokens { get; private set; } = new();
    public int TokenVersion { get; private set; } = 0;
    public string? EmailVerificationToken { get; private set; }
    public DateTime? EmailVerificationTokenExpiresAt { get; private set; }
    public string? PasswordResetOtp { get; private set; }
    public DateTime? PasswordResetOtpExpiresAt { get; private set; }
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public IEnumerable<string> RoleIds => UserRoles.Select(ur => ur.Role.Name);

    //register user part
    public static User RegisterUser(
    string englishName,
    string arabicName,
    string mobile,
    string email,
    string passwordHash,
    string emailVerificationToken,
    DateTime emailVerificationTokenExpiresAt)
    {
        return new User
        {
            Id = Guid.NewGuid()/*.ToString()*/,
            NameEnglish = englishName,
            NameArabic = arabicName,
            Mobile = mobile,
            Email = email,
            PasswordHash = passwordHash,
            IsActive = false,
            CreatedBy = null,
            CreatedDate = DateTime.Now,
            EmailVerificationToken = emailVerificationToken,
            EmailVerificationTokenExpiresAt = emailVerificationTokenExpiresAt
        };
    }

    public void AddRole(Guid roleId)
    {
        if (UserRoles.Any(r => r.RoleId == roleId))
            return;

        UserRoles.Add(new UserRole
        {
            UserId = Id,
            RoleId = roleId
        });
    }

    public void UpdateVerifiedUser()
    {
        EmailVerificationToken = null;
        EmailVerificationTokenExpiresAt = null;
        //RoleIds = ["Default"];
        IsActive = true;
    }

    public void UpdateUserEmailReVerification(string token, DateTime expiresAt)
    {
        EmailVerificationToken = token;
        EmailVerificationTokenExpiresAt = expiresAt;
    }

    public void UpdatePasswordResetOtp(string otp, DateTime expiresAt)
    {
        PasswordResetOtp = otp;
        PasswordResetOtpExpiresAt = expiresAt;
    }

    public void UpdateUserPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        PasswordResetOtp = null;
        PasswordResetOtpExpiresAt = null;
        TokenVersion++;
    }

    public void IncrementFailedAttempts()
    {
        FailedLoginAttempts++;
    }

    public void ResetFailedAttempts()
    {
        FailedLoginAttempts = 0;
        LockedDate = null;
    }

    public void LockAccount()
    {
        IsLocked = true;
        LockedDate = DateTime.UtcNow;
    }

    public void UpdateLockStatus(Guid id, bool isLocked)
    {
        Id = id;
        IsLocked = isLocked;
        LockedDate = IsLocked ? DateTime.UtcNow : null;
    }

    public void ClearRoles()
    {
        UserRoles.Clear();
    }

    public void ClearUserRoles()
    {
        foreach (var role in UserRoles.ToList())
        {
            UserRoles.Remove(role);
        }
    }

    public void Update(
        string nameArabic,
        string nameEnglish,
        string mobile,
        string address,
        string email,
        Gender gender,
        bool isLocked,
        int failedLoginAttempts,
        DateTime? lockedDate)
    {
        NameArabic = nameArabic;
        NameEnglish = nameEnglish;
        Mobile = mobile;
        Address = address;
        Email = email;
        Gender = gender;
        IsLocked = isLocked;
        FailedLoginAttempts = failedLoginAttempts;
        LockedDate = lockedDate;
        UpdatedDate = DateTime.Now;
    }

    public static User CreateByAdmin(string nameArabic, string nameEnglish, string mobile, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            NameEnglish = nameEnglish,
            NameArabic = nameArabic,
            Mobile = mobile,
            Email = email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedBy = 1,
            CreatedDate = DateTime.Now
        };
    }
}
