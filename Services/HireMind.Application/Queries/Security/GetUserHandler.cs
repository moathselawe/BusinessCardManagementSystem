namespace HireMind.Application.Queries.Seurity;
public record GetUserByIdQuery(Guid Id) : IRequest<GetUserByIdResult>;
public record GetUserByIdResult(UserResponseDto response);
public class GetUserByIdHandlerValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
internal class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return new GetUserByIdResult(null!);

        //var dto = entity.Adapt<GetUserResponseDto>();//
        var dto = new UserResponseDto
        {
            Id = entity.Id,
            NameArabic = entity.NameArabic,
            NameEnglish = entity.NameEnglish,
            Email = entity.Email,
            Mobile = entity.Mobile,
            Address = entity.Address,
            Gender = entity.Gender,
            IsActive = entity.IsActive,
            IsLocked = entity.IsLocked,
            FailedLoginAttempts = entity.FailedLoginAttempts,
            LockedDate = entity.LockedDate,
            RoleIds = entity.UserRoles?.Select(x => x.RoleId).ToList() ?? new List<Guid>()
        };
        return new GetUserByIdResult(dto);
    }
}
