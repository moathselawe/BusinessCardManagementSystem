namespace BCMS.Domain.SeedWork;

public abstract class BaseAuditableEntity : BaseEntity
{
    public virtual bool IsDeleted { get; set; }
    public virtual Guid? DeletedByUserId { get; set; }
    public virtual DateTime? DeleteDate { get; set; }
    public virtual Guid? CreatedByUserId { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual Guid? LastModifiedByUserId { get; set; }
    public virtual DateTime? LastModifiedDate { get; set; }
}
