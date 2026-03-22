namespace HireMind.Domain.Dtos.ApplicationStage;
public class SearchJobApplicationsRequestDto
{
    public int JobId { get; set; }
    public int? StageId { get; set; }
    public int? StageStatusId { get; set; }
    public int? Limit { get; set; }
    public string? SearchInput { get; set; }
}
