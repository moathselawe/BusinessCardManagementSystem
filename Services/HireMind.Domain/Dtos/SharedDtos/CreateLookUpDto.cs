namespace HireMind.Domain.Dtos.SharedDtos;

public record CreateLookUpDto
{
    public string CategoryName { get; set; } = default!;
    public Guid? ParentId { get; set; }
}