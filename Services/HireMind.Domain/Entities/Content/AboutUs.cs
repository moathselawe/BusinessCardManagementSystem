namespace HireMind.Domain.Entities.Content;
public class AboutUs : BaseAuditableEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; } = true;
}