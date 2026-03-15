namespace HireMind.Domain.Dtos.SharedDtos;

public record UpdateLookUpDto
{
    public int Id { get; set; }                 
    public string CategoryName { get; set; } = default!;
    public int? ParentId { get; set; }         
}