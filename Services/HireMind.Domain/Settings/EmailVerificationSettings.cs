namespace HireMind.Domain.Settings;
public class EmailVerificationSettings
{
    public int TokenExpirationMinutes { get; set; }
    public string VerifyLink { get; set; } = string.Empty;
}