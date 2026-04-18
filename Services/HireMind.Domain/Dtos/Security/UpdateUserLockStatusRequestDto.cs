namespace HireMind.Domain.Dtos.Security;

public class UpdateUserLockStatusRequestDto
{
    public Guid Id { get; set; }
    public bool IsLocked { get; set; } = true;
}
