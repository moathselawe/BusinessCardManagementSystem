namespace HireMind.Application.Queries.Seurity;
public record GetAllRolesQuery() : IRequest<GetAllRolesResult>;
public record GetAllRolesResult(List<RoleResponseDto> Roles);

internal class GetAllRolesHandler: IRequestHandler<GetAllRolesQuery, GetAllRolesResult>
{
    private readonly IRoleRepository _repository;

    public GetAllRolesHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAllRolesResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _repository.GetAllAsync(cancellationToken);

        var dtos = roles.Adapt<List<RoleResponseDto>>();

        return new GetAllRolesResult(dtos);
    }
}