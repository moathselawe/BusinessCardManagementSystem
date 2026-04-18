namespace HireMind.Domain.Dtos.Security;

public class RoleRqDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<Guid> PermissionIds { get; set; } = new();
}
