namespace HireMind.Domain.Dtos.Security;
public class UpdateRolePermissionsRqDto
{
    public Guid RoleId { get; set; }

    public List<Guid> PermissionIds { get; set; } = new();
}