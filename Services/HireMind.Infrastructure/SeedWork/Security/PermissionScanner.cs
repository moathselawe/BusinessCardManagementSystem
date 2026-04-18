using HireMind.Application.Security.Permissions;
using System.Reflection;

namespace HireMind.Infrastructure.SeedWork.Security;
public static class PermissionScanner
{
    public static List<string> GetAllPermissions()
    {
        var permissions = new List<string>();

        var types = typeof(PermissionConstants).GetNestedTypes();

        foreach (var type in types)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var value = field.GetValue(null)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    permissions.Add(value);
            }
        }

        return permissions;
    }
}
