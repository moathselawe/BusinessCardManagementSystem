namespace BCMS.Domain.Dtos.ManageJobs;

public class UpdateJobActivationRequestDto
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
}
