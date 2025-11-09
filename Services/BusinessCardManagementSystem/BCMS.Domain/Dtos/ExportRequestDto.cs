namespace BCMS.Domain.Dtos;
    public class ExportRequestDto
{
    public string FileType { get; set; } = "csv";
    public List<Guid>? Ids { get; set; } 
}
