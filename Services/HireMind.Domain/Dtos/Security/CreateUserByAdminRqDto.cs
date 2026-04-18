namespace HireMind.Domain.Dtos.Security;

public class CreateUserByAdminRqDto
{
    public string NameEnglish { get; set; }
    public string NameArabic { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public List<Guid> RoleIds { get; set; } 
}