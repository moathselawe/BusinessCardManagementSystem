namespace HireMind.Domain.Entities.Security;
public class Role : Entity<string>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<string> PrivilegeIds { get; set; } = new(); 
}
