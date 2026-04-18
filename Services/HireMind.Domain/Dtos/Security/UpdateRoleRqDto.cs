namespace HireMind.Domain.Dtos.Security;

public class UpdateRoleRqDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public List<Guid> PermissionIds { get; set; } = new();
}