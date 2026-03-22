using HireMind.Domain.Entities.Shared;

namespace HireMind.Domain.Dtos.SharedDtos;
public record GetLookupDto
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }

    public static GetLookupDto FromEntity(Lookup entity, string? parentName)
    {
        return new GetLookupDto
        {
            Id = entity.Id,
            CategoryName = entity.CategoryName,
            ParentId = entity.ParentId,
            ParentName = parentName
        };
    }
}