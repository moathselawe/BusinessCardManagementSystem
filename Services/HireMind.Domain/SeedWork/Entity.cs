namespace HireMind.Domain.SeedWork;

public abstract class Entity<T>
{
    public virtual T Id { get; set; } = default!;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; } = 1;
    public bool IsRemoved { get; set; } = false;
}

