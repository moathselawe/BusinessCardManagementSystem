namespace HireMind.Domain.Dtos.SharedDtos;

public record GetLookupByNameDto
{
    public Guid Id { get; set; }
    public string? CategoryName { get; set; }
}