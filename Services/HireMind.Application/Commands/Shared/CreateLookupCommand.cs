using HireMind.Domain.Entities.Shared;

namespace HireMind.Application.Commands.Shared;

public record CreateLookupCommand(CreateLookUpDto command) : IRequest<CreateLookupResult>;
public record CreateLookupResult(int Id);

internal class CreateLookupHandler : IRequestHandler<CreateLookupCommand, CreateLookupResult>
{
    private readonly ILookupRepository _lookupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLookupHandler(ILookupRepository lookupRepository, IUnitOfWork unitOfWork)
    {
        _lookupRepository = lookupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateLookupResult> Handle(CreateLookupCommand request, CancellationToken cancellationToken)
    {
        var lookup = Lookup.Create(
            categoryName: request.command.CategoryName,
            parentId: request.command.ParentId
        );

        var lookupId = await _lookupRepository.AddAsync(lookup, cancellationToken);
        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new CreateLookupResult(lookupId);
    }

}