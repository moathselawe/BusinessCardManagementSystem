namespace HireMind.Domain.Dtos.Security;

public class UpdateUserRequestDto
{
    public Guid Id { get; set; }
    public string NameArabic { get; set; } 
    public string NameEnglish { get; set; }
    public string Mobile { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public bool IsLocked { get; set; }
    public List<Guid> RoleIds { get; set; }
}
