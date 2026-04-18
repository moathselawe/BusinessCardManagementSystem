using HireMind.Domain.Entities.Security;

namespace HireMind.Infrastructure.SeedWork.Security;
//حتى لا تضطر لإضافة roles يدوياً في database.
//عند تشغيل المشروع لأول مرة:
//سيتم إنشاؤهم تلقائياً.
public static class RoleSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return;

        var roles = new List<Role>
        {
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "SuperAdmin"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "HRManager"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Recruiter"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "JobManager"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Candidate"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "AIUser"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "BusinessCardManager"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Support"
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }


    //يعطي Admin كل الصلاحيات الموجودة في النظام.
    public static async Task AssignAdminPermissions(ApplicationDbContext context)
    {
        var adminRole = await context.Roles
            .FirstAsync(x => x.Name == "Admin");

        var permissions = await context.Permissions.ToListAsync();

        foreach (var permission in permissions)
        {
            var exists = await context.RolePermissions
                .AnyAsync(x =>
                    x.RoleId == adminRole.Id &&
                    x.PermissionId == permission.Id);

            if (!exists)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = adminRole.Id,
                    PermissionId = permission.Id
                });
            }
        }

        await context.SaveChangesAsync();
    }
}