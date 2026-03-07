namespace BCMS.Application.Services;
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
        List<JobFieldDto> jobFields,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Extract Clean Text From File
        string cvText = await ExtractTextFromFileAsync(file, cancellationToken);

        if (string.IsNullOrWhiteSpace(cvText))
            throw new Exception("Could not extract text from CV.");

        // 2️⃣ Clean & Limit Size (CRITICAL)
        cvText = SanitizeText(cvText);

        const int maxCharacters = 12000; // Safe limit
        if (cvText.Length > maxCharacters)
            cvText = cvText[..maxCharacters];

        // 3️⃣ Build Dynamic Fields Description
        var fieldsDescription = string.Join("\n", jobFields.Select(f =>
            $"- {f.FieldName} ({f.FieldType}) Required: {f.IsRequired}"
        ));

        // 4️⃣ Strict System Prompt
        var systemPrompt = $"""
You are a professional CV parsing AI.

Extract ONLY the following fields from the CV.

Return STRICT valid JSON object.
Do NOT add explanation.
If field not found return null.
For list types return JSON array.

Fields:
{fieldsDescription}
""";

        var payload = new
        {
            model = "openai/gpt-4o-mini",
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = cvText }
            },
            temperature = 0.2,
            response_format = new { type = "json_object" }
        };

        var json = JsonSerializer.Serialize(payload);

        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "https://openrouter.ai/api/v1/chat/completions");

        httpRequest.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        httpRequest.Headers.Add("HTTP-Referer", "http://localhost");
        httpRequest.Headers.Add("X-Title", "BCMS CV Analyzer");

        httpRequest.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

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

        // 5️⃣ Parse AI JSON
        var parsedData = ParseJsonSafely(content);

        // 6️⃣ Validate Against jobFields (NO HALLUCINATIONS)
        var validatedData = new Dictionary<string, object?>();

        foreach (var field in jobFields)
        {
            if (parsedData.TryGetValue(field.FieldName, out var value))
                validatedData[field.FieldName] = value;
            else
                validatedData[field.FieldName] = null;
        }

        return new AnalyzedCvDataDto
        {
            Fields = validatedData
        };
    }

    // -----------------------------
    // TEXT EXTRACTION SECTION
    // -----------------------------

    private async Task<string> ExtractTextFromFileAsync(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        if (file.ContentType.Contains("pdf"))
            return ExtractPdfText(stream);

        if (file.ContentType.Contains("word") ||
            file.FileName.EndsWith(".docx"))
            return ExtractDocxText(stream);

        // fallback for txt
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private string ExtractPdfText(Stream stream)
    {
        var sb = new StringBuilder();

        using (var pdf = PdfDocument.Open(stream))
        {
            foreach (var page in pdf.GetPages())
            {
                sb.AppendLine(page.Text);
            }
        }

        return sb.ToString();
    }

    private string ExtractDocxText(Stream stream)
    {
        using var wordDoc = WordprocessingDocument.Open(stream, false);
        var body = wordDoc.MainDocumentPart?.Document.Body;
        return body?.InnerText ?? "";
    }

    private string SanitizeText(string text)
    {
        text = text.Replace("\0", " ");
        text = text.Replace("\r", " ");
        text = text.Replace("\t", " ");
        return text.Trim();
    }

    // -----------------------------
    // JSON PARSING SECTION
    // -----------------------------

    private Dictionary<string, object?> ParseJsonSafely(string content)
    {
        var result = new Dictionary<string, object?>();

        try
        {
            using var jsonDoc = JsonDocument.Parse(content);

            foreach (var prop in jsonDoc.RootElement.EnumerateObject())
            {
                result[prop.Name] = ConvertJsonElement(prop.Value);
            }
        }
        catch
        {
            // if AI breaks JSON format
            return new Dictionary<string, object?>();
        }

        return result;
    }

    private object? ConvertJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Array => element.EnumerateArray()
                                          .Select(ConvertJsonElement)
                                          .ToList(),
            JsonValueKind.Null => null,
            _ => element.ToString()
        };
    }
}
