namespace HireMind.Domain.Entities.Security;
public class Role : Entity<Guid>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public static Role Create(string name, string? description)
    {
        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }

    public void AddPermissions(List<Guid> permissionIds)
    {
        foreach (var id in permissionIds)
        {
            RolePermissions.Add(new RolePermission
            {
                RoleId = Id,
                PermissionId = id
            });
        }
    }

    public void Update(string name, string? description, List<Guid> permissionIds)
    {
        Name = name;
        Description = description;

        RolePermissions.Clear();

        foreach (var permissionId in permissionIds)
        {
            RolePermissions.Add(new RolePermission
            {
                RoleId = Id,
                PermissionId = permissionId
            });
        }
    }

    public void UpdatePermissions(List<Guid> permissionIds)
    {
        RolePermissions.Clear();

        foreach (var permissionId in permissionIds)
        {
            RolePermissions.Add(new RolePermission
            {
                RoleId = Id,
                PermissionId = permissionId
            });
        }
    }
}
