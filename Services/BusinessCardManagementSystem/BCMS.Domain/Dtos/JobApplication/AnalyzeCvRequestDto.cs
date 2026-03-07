namespace BCMS.Domain.Dtos.JobApplication;
public class AnalyzeCvRequestDto
{
    public IFormFile File { get; set; }   
    public Guid JobId { get; set; }    
}