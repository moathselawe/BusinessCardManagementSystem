namespace HireMind.Domain.Dtos.JobApplication;

public class SubmitJobApplicationRequestDto
{
    public int JobId { get; set; }
    public int? AnalyzeCvId { get; set; }
    public Dictionary<string, object?> Answers { get; set; } = new();
    public PersonalInfoDto PersonalInfo { get; set; } = new PersonalInfoDto();
   // public string EmailAddress { get; set; } = null;
}



public class PersonalInfoDto
{
    public string FullName { get; set; } = null!;
    public string MobileNumber { get; set; } = null!;
    public int CountryCodeId { get; set; }
    public string EmailAddress { get; set; } = null!;
}