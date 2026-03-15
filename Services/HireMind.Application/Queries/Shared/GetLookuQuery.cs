namespace HireMind.Application.Queries.Shared;
public record GetLookupByIdQuery(int Id) : IRequest<GetLookupByIdResult>;
public record GetLookupByIdResult(GetLookupDto response);
public class GetLookupByIdHandlerValidator : AbstractValidator<GetLookupByIdQuery>
{
    public GetLookupByIdHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
internal class GetLookupByIdHandler : IRequestHandler<GetLookupByIdQuery, GetLookupByIdResult>
{
    private readonly ILookupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GetLookupByIdHandler(ILookupRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetLookupByIdResult> Handle(GetLookupByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdWithParentAsync(request.Id, cancellationToken);

        if (entity == null)
            return new GetLookupByIdResult(null!);

        var dto = new GetLookupDto
        {
            Id = entity.Id,
            CategoryName = entity.CategoryName,
            ParentId = entity.ParentId,
            ParentName = entity.Parent?.CategoryName
        };

        return new GetLookupByIdResult(dto);
    }

    //public async Task<GetLookupByIdResult> Handle(GetLookupByIdQuery request, CancellationToken cancellationToken)
    //{
    //    var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

    //    if (entity == null)
    //        return new GetLookupByIdResult(null!);

    //    string? parentName = null;

    //    if (entity.ParentId.HasValue)
    //    {
    //        var parent = await _repository.GetByIdAsync(entity.ParentId.Value, cancellationToken);
    //        parentName = parent?.CategoryName;
    //    }

    //    var dto = GetLookupDto.FromEntity(entity, parentName);

    //    return new GetLookupByIdResult(dto);
    //}
}
