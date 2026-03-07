namespace HireMind.Domain.Dtos.SharedDtos;

public record SearchFiltersRsDto<T>(List<T> Items, long TotalCount);
