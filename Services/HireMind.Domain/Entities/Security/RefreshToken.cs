namespace HireMind.Domain.Entities.Security;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string TokenHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool Revoked { get; set; } = false;

    public string? Ip { get; set; }

    public string? UserAgent { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsActive => !Revoked && !IsExpired;
}