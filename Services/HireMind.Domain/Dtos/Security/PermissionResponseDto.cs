namespace HireMind.Domain.Dtos.Security;
public class PermissionResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }
}