namespace HireMind.Domain.Dtos.JobApplication;
public class JobApplicationDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public int CountryCodeId { get; set; }
    public string? CountryCode { get; set; }
    public double SystemScore { get; set; }
    public double TotalScore { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CurrentStageName { get; set; } = string.Empty;
    public int CurrentStageOrder { get; set; }
    public int ApplicationStageId { get; set; }
    public int HiringStageId { get; set; }
    public string Status { get; set; } = string.Empty;
}