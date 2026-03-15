namespace HireMind.Domain.Dtos.ManageJobs;

public class UpdateJobActivationRequestDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
}
