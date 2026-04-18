namespace HireMind.Domain.Dtos.Security;

public class GetUserResponseDto
{
    public Guid Id { get; set; }

    public string NameArabic { get; set; } = null!;
    public string NameEnglish { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Mobile { get; set; } = null!;

    public string? Address { get; set; }

    public Gender Gender { get; set; } 

    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? LockedDate { get; set; }

    public List<string> RoleIds { get; set; } = new();
}