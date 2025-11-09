namespace BCMS.Domain.Dtos;

public record SearchFiltersRsDto<T>(List<T> Items, long TotalCount);
