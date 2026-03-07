namespace HireMind.Application.Commands.Shared;

public record UpdateLookupCommand(UpdateLookUpDto request) : IRequest<UpdateLookupResult>;
public record UpdateLookupResult(bool IsSuccess);

public class UpdateLookupHandler : IRequestHandler<UpdateLookupCommand, UpdateLookupResult>
{
    private readonly ILookupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLookupHandler(ILookupRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UpdateLookupResult> Handle(UpdateLookupCommand command, CancellationToken cancellationToken)
    {
        var Lookup = lookupModel.Update(
                id: command.request.Id,
                categoryName: command.request.CategoryName,
                parentId: command.request.ParentId
            );


        var result = await _repository.UpdateAsync(Lookup, cancellationToken);

        await _unitOfWork.SaveWorkAsync(cancellationToken);

        return new UpdateLookupResult(result);
    }
}

