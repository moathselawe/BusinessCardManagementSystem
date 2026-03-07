namespace BCMS.Domain.Dtos.SharedDtos;

public record SearchFiltersRsDto<T>(List<T> Items, long TotalCount);
