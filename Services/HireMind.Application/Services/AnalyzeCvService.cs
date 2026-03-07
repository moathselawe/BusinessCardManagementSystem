using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DocumentFormat.OpenXml.Packaging;
using UglyToad.PdfPig;
using HireMind.Domain.Entities;

namespace HireMind.Application.Services;

public class AnalyzeCvService : IAnalyzeCvService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AnalyzeCvService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"];
    }

    public async Task<AnalyzedCvDataDto> GetAnalyzedCvAsync(
        IFormFile file,
        Job job,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Extract CV text
        var cvText = await ExtractTextFromFileAsync(file, cancellationToken);

        if (string.IsNullOrWhiteSpace(cvText))
            throw new Exception("Could not extract text from CV.");

        cvText = SanitizeText(cvText);

        const int maxChars = 12000;

        if (cvText.Length > maxChars)
            cvText = cvText[..maxChars];

        // 2️⃣ Build Job Context
        var jobContext = BuildJobContext(job);

        // 3️⃣ Build JSON fields structure
        var jsonFields = BuildJsonSchema(job);

        // 4️⃣ Prompt
        var systemPrompt = """
You are an expert recruitment AI.

Your task is to analyze a candidate CV and answer job application questions.

Rules:

- Use ONLY information from the CV
- Do NOT hallucinate
- If the answer is not in the CV return null
- If the question has options choose the closest option
- Return STRICT JSON only
- Do NOT include explanation text
""";

        var payload = new
        {
            model = "openai/gpt-4o-mini",
            messages = new object[]
            {
                new { role = "system", content = systemPrompt },

                new
                {
                    role = "user",
                    content = $"JOB INFORMATION:\n{jobContext}"
                },

                new
                {
                    role = "user",
                    content = $"Return JSON using this structure:\n{jsonFields}"
                },

                new
                {
                    role = "user",
                    content = $"CANDIDATE CV:\n{cvText}"
                }
            },
            temperature = 0.1,
            response_format = new { type = "json_object" }
        };

        var json = JsonSerializer.Serialize(payload);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://openrouter.ai/api/v1/chat/completions");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        request.Headers.Add("HTTP-Referer", "http://localhost");

        request.Headers.Add("X-Title", "HireMind CV Analyzer");

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(
            request,
            cancellationToken);

        var responseString = await response.Content
            .ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"AI Error: {responseString}");

        using var doc = JsonDocument.Parse(responseString);

        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content))
            return new AnalyzedCvDataDto();

        var parsed = ParseJsonSafely(content);

        var validated = ValidateAnswers(parsed, job);

        return new AnalyzedCvDataDto
        {
            Fields = validated
        };
    }

    // ------------------------------------------------
    // BUILD JOB CONTEXT
    // ------------------------------------------------

    private string BuildJobContext(Job job)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Title: {job.Title}");
        sb.AppendLine($"Description: {job.Description}");
        sb.AppendLine("Questions:");

        for (int i = 0; i < job.Questions.Count; i++)
        {
            var q = job.Questions[i];

            sb.AppendLine($"{i}: {q.QuestionText}");

            if (q.AvailableAnswers.Any())
            {
                sb.AppendLine("Options:");

                foreach (var option in q.AvailableAnswers)
                    sb.AppendLine($" - {option.Text}");
            }
        }

        return sb.ToString();
    }

    // ------------------------------------------------
    // BUILD JSON STRUCTURE
    // ------------------------------------------------

    private string BuildJsonSchema(Job job)
    {
        var dict = new Dictionary<string, object?>();

        for (int i = 0; i < job.Questions.Count; i++)
            dict.Add($"Q_{i}", null);

        return JsonSerializer.Serialize(dict, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    // ------------------------------------------------
    // VALIDATION
    // ------------------------------------------------

    private Dictionary<string, object?> ValidateAnswers(
        Dictionary<string, object?> parsed,
        Job job)
    {
        var result = new Dictionary<string, object?>();

        for (int i = 0; i < job.Questions.Count; i++)
        {
            var key = $"Q_{i}";

            if (parsed.TryGetValue(key, out var value))
                result[key] = value;
            else
                result[key] = null;
        }

        return result;
    }

    // ------------------------------------------------
    // FILE EXTRACTION
    // ------------------------------------------------

    private async Task<string> ExtractTextFromFileAsync(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();

        await file.CopyToAsync(stream, cancellationToken);

        stream.Position = 0;

        if (file.ContentType.Contains("pdf"))
            return ExtractPdfText(stream);

        if (file.FileName.EndsWith(".docx"))
            return ExtractDocxText(stream);

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private string ExtractPdfText(Stream stream)
    {
        var sb = new StringBuilder();

        using var pdf = PdfDocument.Open(stream);

        foreach (var page in pdf.GetPages())
            sb.AppendLine(page.Text);

        return sb.ToString();
    }

    private string ExtractDocxText(Stream stream)
    {
        using var word = WordprocessingDocument.Open(stream, false);

        var body = word.MainDocumentPart?.Document.Body;

        return body?.InnerText ?? "";
    }

    private string SanitizeText(string text)
    {
        text = text.Replace("\0", " ");
        text = text.Replace("\r", " ");
        text = text.Replace("\t", " ");

        return text.Trim();
    }

    // ------------------------------------------------
    // JSON PARSER
    // ------------------------------------------------

    private Dictionary<string, object?> ParseJsonSafely(string content)
    {
        var result = new Dictionary<string, object?>();

        try
        {
            using var doc = JsonDocument.Parse(content);

            foreach (var prop in doc.RootElement.EnumerateObject())
                result[prop.Name] = ConvertJson(prop.Value);
        }
        catch
        {
            return new Dictionary<string, object?>();
        }

        return result;
    }

    private object? ConvertJson(JsonElement el)
    {
        return el.ValueKind switch
        {
            JsonValueKind.String => el.GetString(),
            JsonValueKind.Number => el.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Array => el.EnumerateArray()
                .Select(ConvertJson)
                .ToList(),
            JsonValueKind.Null => null,
            _ => el.ToString()
        };
    }
}


//namespace HireMind.Application.Services;
//public class AnalyzeCvService : IAnalyzeCvService
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _apiKey;

//    public AnalyzeCvService(HttpClient httpClient, IConfiguration configuration)
//    {
//        _httpClient = httpClient;
//        _apiKey = configuration["OpenAI:ApiKey"];
//    }

//    public async Task<AnalyzedCvDataDto> GetAnalyzedCvAsync(
//        IFormFile file,
//        List<JobFieldDto> jobFields,
//        CancellationToken cancellationToken = default)
//    {
//        // 1️⃣ Extract Clean Text From File
//        string cvText = await ExtractTextFromFileAsync(file, cancellationToken);

//        if (string.IsNullOrWhiteSpace(cvText))
//            throw new Exception("Could not extract text from CV.");

//        // 2️⃣ Clean & Limit Size (CRITICAL)
//        cvText = SanitizeText(cvText);

//        const int maxCharacters = 12000; // Safe limit
//        if (cvText.Length > maxCharacters)
//            cvText = cvText[..maxCharacters];

//        // 3️⃣ Build Dynamic Fields Description
//        var fieldsDescription = string.Join("\n", jobFields.Select(f =>
//            $"- {f.FieldName} ({f.FieldType}) Required: {f.IsRequired}"
//        ));

//        // 4️⃣ Strict System Prompt
//        var systemPrompt = $"""
//You are a professional CV parsing AI.

//Extract ONLY the following fields from the CV.

//Return STRICT valid JSON object.
//Do NOT add explanation.
//If field not found return null.
//For list types return JSON array.

//Fields:
//{fieldsDescription}
//""";

//        var payload = new
//        {
//            model = "openai/gpt-4o-mini",
//            messages = new[]
//            {
//                new { role = "system", content = systemPrompt },
//                new { role = "user", content = cvText }
//            },
//            temperature = 0.2,
//            response_format = new { type = "json_object" }
//        };

//        var json = JsonSerializer.Serialize(payload);

//        var httpRequest = new HttpRequestMessage(
//            HttpMethod.Post,
//            "https://openrouter.ai/api/v1/chat/completions");

//        httpRequest.Headers.Authorization =
//            new AuthenticationHeaderValue("Bearer", _apiKey);

//        httpRequest.Headers.Add("HTTP-Referer", "http://localhost");
//        httpRequest.Headers.Add("X-Title", "HireMind CV Analyzer");

//        httpRequest.Content = new StringContent(
//            json,
//            Encoding.UTF8,
//            "application/json");

//        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
//        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

//        if (!response.IsSuccessStatusCode)
//            throw new Exception($"AI Error: {responseString}");

//        using var doc = JsonDocument.Parse(responseString);

//        var content = doc.RootElement
//            .GetProperty("choices")[0]
//            .GetProperty("message")
//            .GetProperty("content")
//            .GetString();

//        if (string.IsNullOrWhiteSpace(content))
//            return new AnalyzedCvDataDto();

//        // 5️⃣ Parse AI JSON
//        var parsedData = ParseJsonSafely(content);

//        // 6️⃣ Validate Against jobFields (NO HALLUCINATIONS)
//        var validatedData = new Dictionary<string, object?>();

//        foreach (var field in jobFields)
//        {
//            if (parsedData.TryGetValue(field.FieldName, out var value))
//                validatedData[field.FieldName] = value;
//            else
//                validatedData[field.FieldName] = null;
//        }

//        return new AnalyzedCvDataDto
//        {
//            Fields = validatedData
//        };
//    }

//    // -----------------------------
//    // TEXT EXTRACTION SECTION
//    // -----------------------------

//    private async Task<string> ExtractTextFromFileAsync(
//        IFormFile file,
//        CancellationToken cancellationToken)
//    {
//        using var stream = new MemoryStream();
//        await file.CopyToAsync(stream, cancellationToken);
//        stream.Position = 0;

//        if (file.ContentType.Contains("pdf"))
//            return ExtractPdfText(stream);

//        if (file.ContentType.Contains("word") ||
//            file.FileName.EndsWith(".docx"))
//            return ExtractDocxText(stream);

//        // fallback for txt
//        return Encoding.UTF8.GetString(stream.ToArray());
//    }

//    private string ExtractPdfText(Stream stream)
//    {
//        var sb = new StringBuilder();

//        using (var pdf = PdfDocument.Open(stream))
//        {
//            foreach (var page in pdf.GetPages())
//            {
//                sb.AppendLine(page.Text);
//            }
//        }

//        return sb.ToString();
//    }

//    private string ExtractDocxText(Stream stream)
//    {
//        using var wordDoc = WordprocessingDocument.Open(stream, false);
//        var body = wordDoc.MainDocumentPart?.Document.Body;
//        return body?.InnerText ?? "";
//    }

//    private string SanitizeText(string text)
//    {
//        text = text.Replace("\0", " ");
//        text = text.Replace("\r", " ");
//        text = text.Replace("\t", " ");
//        return text.Trim();
//    }

//    // -----------------------------
//    // JSON PARSING SECTION
//    // -----------------------------

//    private Dictionary<string, object?> ParseJsonSafely(string content)
//    {
//        var result = new Dictionary<string, object?>();

//        try
//        {
//            using var jsonDoc = JsonDocument.Parse(content);

//            foreach (var prop in jsonDoc.RootElement.EnumerateObject())
//            {
//                result[prop.Name] = ConvertJsonElement(prop.Value);
//            }
//        }
//        catch
//        {
//            // if AI breaks JSON format
//            return new Dictionary<string, object?>();
//        }

//        return result;
//    }

//    private object? ConvertJsonElement(JsonElement element)
//    {
//        return element.ValueKind switch
//        {
//            JsonValueKind.String => element.GetString(),
//            JsonValueKind.Number => element.GetDouble(),
//            JsonValueKind.True => true,
//            JsonValueKind.False => false,
//            JsonValueKind.Array => element.EnumerateArray()
//                                          .Select(ConvertJsonElement)
//                                          .ToList(),
//            JsonValueKind.Null => null,
//            _ => element.ToString()
//        };
//    }
//}
