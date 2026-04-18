using HireMind.Domain.Entities.Security;

namespace HireMind.Infrastructure.SeedWork.Security;
//permissions في الكود
//permissions في قاعدة البيانات
//يضيف أي permission غير موجود
public static class PermissionSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var permissions = PermissionScanner.GetAllPermissions();

        foreach (var permission in permissions)
        {
            var exists = await context.Permissions
                .AnyAsync(p => p.Code == permission);

            if (!exists)
            {
                context.Permissions.Add(new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = permission,
                    Code = permission,
                    Description = permission,
                    CreatedDate = DateTime.Now
                });
            }
        }

        await context.SaveChangesAsync();
    }
}