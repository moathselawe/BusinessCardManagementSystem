namespace HireMind.Domain.Dtos.JobApplication;
public class AnalyzeCvRequestDto
{
    public IFormFile File { get; set; }   
    public int JobId { get; set; }    
    public string? EmailAddress { get; set; }    
}