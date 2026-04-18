namespace HireMind.Domain.Dtos.Security;
public class UpdateUserRolesRqDto
{
    public Guid UserId { get; set; }

    public List<Guid> RoleIds { get; set; } = new();
}