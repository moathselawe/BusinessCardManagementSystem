namespace HireMind.Domain.Dtos.SharedDtos;

public record UpdateLookUpDto
{
    public Guid Id { get; set; }                 
    public string CategoryName { get; set; } = default!;
    public Guid? ParentId { get; set; }         
}