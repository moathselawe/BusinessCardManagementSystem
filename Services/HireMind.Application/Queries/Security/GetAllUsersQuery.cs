namespace HireMind.Application.Queries.Seurity;
public record GetAllUsersQuery() : IRequest<GetAllUsersResult>;
public record GetAllUsersResult(List<UserResponseDto> Users);

internal class GetAllUsersHandler: IRequestHandler<GetAllUsersQuery, GetAllUsersResult>
{
    private readonly IUserRepository _repository;

    public GetAllUsersHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAllUsersResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var Users = await _repository.GetAllAsync(cancellationToken);

        var dtos = Users.Adapt<List<UserResponseDto>>();

        return new GetAllUsersResult(dtos);
    }
}