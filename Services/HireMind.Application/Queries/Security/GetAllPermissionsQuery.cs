namespace HireMind.Application.Queries.Seurity;
public record GetAllPermissionsQuery() : IRequest<GetAllPermissionsResult>;
public record GetAllPermissionsResult(List<PermissionResponseDto> Permissions);

internal class GetAllPermissionsHandler: IRequestHandler<GetAllPermissionsQuery, GetAllPermissionsResult>
{
    private readonly IPermissionRepository _repository;

    public GetAllPermissionsHandler(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAllPermissionsResult> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var Permissions = await _repository.GetAllAsync(cancellationToken);

        var dtos = Permissions.Adapt<List<PermissionResponseDto>>();

        return new GetAllPermissionsResult(dtos);
    }
}