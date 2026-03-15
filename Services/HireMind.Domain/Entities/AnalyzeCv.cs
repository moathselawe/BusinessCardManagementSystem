using System.Net.Mail;

namespace HireMind.Domain.Entities;

public class AnalyzeCv : BaseAuditableEntity
{
    public int JobId { get; private set; }
    public Job Job { get; private set; } = null!;

    public string CvFilePath { get; private set; } = null!;
    public string CvText { get; private set; } = null!;
    public double AiScore { get; private set; }
    public string ExtractedAnswersJson { get; private set; } = "{}";
    public bool IsActive { get; private set; }
    public string? EmailAddress { get; private set; }


    public static AnalyzeCv Create(int jobId, string cvFilePath, string cvText, double aiScore, string extractedAnswersJson, bool isActive,string emailAddress)
    {
        return new AnalyzeCv()
        {
            JobId = jobId,
            CvFilePath = cvFilePath,
            CvText = cvText,
            AiScore = aiScore,
            ExtractedAnswersJson = extractedAnswersJson,
            IsActive = isActive,
            EmailAddress = emailAddress,
            CreatedDate = DateTime.Now
        };
    }

    public static AnalyzeCv Update(int id, int jobId, string cvFilePath, string cvText, double aiScore, string extractedAnswersJson, bool isActive, string emailAddress)
    {
        return new AnalyzeCv()
        {
            Id = id,
            JobId = jobId,
            CvFilePath = cvFilePath,
            CvText = cvText,
            AiScore = aiScore,
            ExtractedAnswersJson = extractedAnswersJson,
            IsActive = isActive,
            EmailAddress = emailAddress,
            LastModifiedDate = DateTime.Now
        };
    }

    //public static AnalyzeCv UpdateActivation(int id, bool isActive)
    //{
    //    return new AnalyzeCv()
    //    {
    //        Id = id,
    //        IsActive = isActive,
    //        LastModifiedDate = DateTime.Now
    //    };
    //}

    public void Activate()
    {
        IsActive = true;
        LastModifiedDate = DateTime.Now;
    }

}