namespace HireMind.Domain.Dtos.JobApplication;

public class GetJobApplicationByIdDto
{
    public int Id { get; set; }

    // User Answers
    public string UserAnswersJson { get; set; } = "[]"; 
    public Dictionary<string, object> UserAnswers
    {
        get
        {
            return string.IsNullOrEmpty(UserAnswersJson)
                ? new Dictionary<string, object>()
                : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(UserAnswersJson)!;
        }
    }

    // Personal Info
    public PersonalInfoDto PersonalInfo { get; set; } = new PersonalInfoDto();
}
