namespace HireMind.Domain.Dtos.AI;
public record AISuggestionsRequestDto(SuggestionType SuggestionType, string Input);

public enum SuggestionType
{
    Countries = 1,
    NameArabic = 2,
    NameEnglish = 3,
    Email = 4,
    Address = 5,
}