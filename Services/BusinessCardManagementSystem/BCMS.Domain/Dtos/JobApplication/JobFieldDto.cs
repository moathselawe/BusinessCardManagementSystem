namespace BCMS.Domain.Dtos.JobApplication;
public class JobFieldDto
{
    public string FieldName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? FieldType { get; set; }
}