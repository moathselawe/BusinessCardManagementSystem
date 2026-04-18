namespace HireMind.Domain.Dtos.Security;

public class GetRoleResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<string> PermissionIds { get; set; } = new();
}
