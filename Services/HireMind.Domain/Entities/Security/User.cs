namespace HireMind.Domain.Entities.Security;
public class User : Entity<string>
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
    public List<string> RoleIds { get; private set; } = new();
    public List<RefreshToken> RefreshTokens { get; private set; } = new();
    public int TokenVersion { get; private set; } = 0;
    public string? EmailVerificationToken { get; private set; }
    public DateTime? EmailVerificationTokenExpiresAt { get; private set; }

    //new
    public string? PasswordResetOtp { get; private set; }
    public DateTime? PasswordResetOtpExpiresAt { get; private set; }

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
            Id = Guid.NewGuid().ToString(),
            NameEnglish = englishName,
            NameArabic = arabicName,
            Mobile = mobile,
            Email = email,
            PasswordHash = passwordHash,
            IsActive = false,
            CreatedBy = 1,
            CreatedDate = DateTime.Now,
            EmailVerificationToken = emailVerificationToken,
            EmailVerificationTokenExpiresAt = emailVerificationTokenExpiresAt
        };
    }

    //public static User UpdateVerifiedUser(User existingUser)
    //{
    //    existingUser.EmailVerificationToken = null;
    //    existingUser.EmailVerificationTokenExpiresAt = null;
    //    existingUser.RoleIds = ["Default"];
    //    existingUser.IsActive = true;
    //    return existingUser;
    //}

    public void UpdateVerifiedUser()
    {
        EmailVerificationToken = null;
        EmailVerificationTokenExpiresAt = null;
        RoleIds = ["Default"];
        IsActive = true;
    }

    public void UpdateUserEmailReVerification(string token, DateTime expiresAt)
    {
        EmailVerificationToken = token;
        EmailVerificationTokenExpiresAt = expiresAt;
    }
    //public static User UpdateUserEmailReVerification(
    //User existingUser,
    //string emailVerificationToken,
    //DateTime emailVerificationTokenExpiresAt)
    //{
    //    existingUser.EmailVerificationToken = emailVerificationToken;
    //    existingUser.EmailVerificationTokenExpiresAt = emailVerificationTokenExpiresAt;
    //    return existingUser;
    //}

    //reset password part
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
}
