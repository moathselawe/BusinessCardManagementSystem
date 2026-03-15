namespace HireMind.Domain.Dtos.SharedDtos;
public record GetAllLookupsPartenersAndChildrensDto
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public List<GetLookupDto> Children { get; set; } = new List<GetLookupDto>();
}