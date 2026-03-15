namespace HireMind.Domain.Dtos.SharedDtos;

public record CreateLookUpDto
{
    public string CategoryName { get; set; } = default!;
    public int? ParentId { get; set; }
}