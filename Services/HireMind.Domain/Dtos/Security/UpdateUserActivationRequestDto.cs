namespace HireMind.Domain.Dtos.Security;

public class UpdateUserActivationRequestDto
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
}
