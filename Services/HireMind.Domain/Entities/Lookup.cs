namespace HireMind.Domain.Entities;

public class Lookup : BaseAuditableEntity
{
    public string? CategoryName { get; set; }
    public Guid? ParentId { get; set; }

    public static Lookup Create(string categoryName, Guid? parentId = null)
    {
        return new Lookup()
        {
            CategoryName = categoryName,
            ParentId = parentId,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static Lookup Update(Guid id, string categoryName, Guid? parentId = null)
    {
        return new Lookup()
        {
            Id = id,
            CategoryName = categoryName,
            ParentId = parentId,
            LastModifiedDate = DateTime.UtcNow
        };
    }

}


