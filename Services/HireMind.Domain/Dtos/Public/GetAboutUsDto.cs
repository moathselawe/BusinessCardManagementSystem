namespace HireMind.Domain.Dtos.Public;
public class GetAboutUsDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public int Order { get; set; }

}