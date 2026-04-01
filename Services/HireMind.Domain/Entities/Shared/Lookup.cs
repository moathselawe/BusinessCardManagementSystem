namespace HireMind.Domain.Entities.Shared;

public class Lookup : BaseAuditableEntity
{
    public string CategoryName { get; private set; } = null!;
    public int? ParentId { get; private set; }

    public Lookup? Parent { get; private set; }

    public ICollection<Lookup> Children { get; private set; } = new List<Lookup>();

    public static Lookup Create(string categoryName, int? parentId = null)
    {
        return new Lookup
        {
            CategoryName = categoryName,
            ParentId = parentId,
            CreatedDate = DateTime.UtcNow
        };
    }

    public static Lookup Update(int id, string categoryName, int? parentId = null)
    {
        return new Lookup
        {
            Id = id,
            CategoryName = categoryName,
            ParentId = parentId,
            LastModifiedDate = DateTime.UtcNow
        };
    }
}
