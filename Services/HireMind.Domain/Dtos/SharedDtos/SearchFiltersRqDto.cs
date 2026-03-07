namespace HireMind.Domain.Dtos.SharedDtos;

public record SearchFiltersRqDto(string? SearchTerm, DateTime? DateSearch = null, int PageNumber = 1, int PageSize = 5, 
    string SortBy = "CreatedDate", string OrderBy = "desc");
