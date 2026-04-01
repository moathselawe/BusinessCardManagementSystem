namespace HireMind.Domain.Entities.Security;

public class Privilege : Entity<string>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
