namespace HireMind.Infrastructure.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public AIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            _model = "gpt-3.5-mini";
        }

        public async Task<ChatbotResponseDto> GetChatbotReplyAsync(
            ChatbotRequestDto request,
            CancellationToken cancellationToken)
        {
            var payload = new
            {
                model = "openai/gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are a helpful assistant." },
            new { role = "user", content = request.Message }
        }
            };

            var json = JsonSerializer.Serialize(payload);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://openrouter.ai/api/v1/chat/completions"); // FULL URL

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            // REQUIRED HEADERS
            httpRequest.Headers.Add("HTTP-Referer", "http://localhost");
            httpRequest.Headers.Add("X-Title", "HireMind Chatbot");

            httpRequest.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            // 🔎 DEBUG: See what OpenRouter actually returns
            if (!response.IsSuccessStatusCode)
            {
                return new ChatbotResponseDto(
                    $"Error {response.StatusCode}: {responseString}");
            }

            if (!response.Content.Headers.ContentType?.MediaType?.Contains("application/json") ?? true)
            {
                return new ChatbotResponseDto(
                    $"Non-JSON response received:\n{responseString}");
            }

            using var doc = JsonDocument.Parse(responseString);

            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return new ChatbotResponseDto(reply ?? "");
        }

        public async Task<AISuggestionsResponseDto> GetAISuggestionsAsync(
            AISuggestionsRequestDto request,
            CancellationToken cancellationToken)
        {
            var systemPrompt = request.SuggestionType switch
            {
                SuggestionType.Countries =>
                    "Return a list of country names that start with or are strongly related to the given input, Only return real country names. Return ONLY a JSON array of strings.",

                SuggestionType.NameArabic =>
                    "Ttranslation only Arabic names based on the input English name. Return ONLY a JSON array of strings.",

                SuggestionType.NameEnglish =>
                    "Translation only English names based on the input Arabic name. Return ONLY a JSON array of strings.",

                SuggestionType.Email =>
                    """       
                    The user provides the beginning of an email address (for example: moath@).
                    Suggest common and valid email domain extensions.
                    Only return domain parts starting with '@'.
                    Return up to 5 results.
                    Return ONLY a valid JSON array of strings.
                    Do not include explanations.
                    Example:
                    ["@gmail.com","@yahoo.com","@outlook.com"] 
                    """,

                SuggestionType.Address =>
                    "Suggest realistic full addresses contains city of country , country of the city based on the input. Return ONLY a JSON array of strings.",

                _ => "Return a JSON array of suggestions."
            };

            var payload = new
            {
                model = "openai/gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = request.Input }
        },
                temperature = 0.3 // lower = more predictable suggestions
            };

            var json = JsonSerializer.Serialize(payload);

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://openrouter.ai/api/v1/chat/completions");

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            httpRequest.Headers.Add("HTTP-Referer", "http://localhost");
            httpRequest.Headers.Add("X-Title", "HireMind Suggestions");

            httpRequest.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return new AISuggestionsResponseDto(
                    new List<string> { $"Error {response.StatusCode}" });
            }

            using var doc = JsonDocument.Parse(responseString);

            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(content))
                return new AISuggestionsResponseDto(new List<string>());

            try
            {
                // AI must return: ["item1","item2","item3"]
                var suggestions = JsonSerializer.Deserialize<List<string>>(content);

                return new AISuggestionsResponseDto(suggestions ?? new List<string>());
            }
            catch
            {
                // Fallback if AI returns bullet list instead of JSON
                var fallback = content
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Replace("-", "").Trim())
                    .ToList();

                return new AISuggestionsResponseDto(fallback);
            }
        }

    }
}
