namespace HireMind.Application.Queries.Security;

public record GetRoleByIdQuery(Guid Id) : IRequest<GetRoleByIdResult>;

public record GetRoleByIdResult(RoleResponseDto response);

public class GetRoleByIdHandlerValidator : AbstractValidator<GetRoleByIdQuery>
{
    public GetRoleByIdHandlerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}

internal class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, GetRoleByIdResult>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetRoleByIdHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRoleByIdResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return new GetRoleByIdResult(null!);

        var dto = new RoleResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,

            PermissionIds = entity.RolePermissions?
                .Select(x => x.PermissionId)
                .ToList() ?? new List<Guid>()
        };

        return new GetRoleByIdResult(dto);
    }
}