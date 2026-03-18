using HireMind.Application.Queries.HiringStages;
using HireMind.Domain.Dtos.UpdateApplicationStageStatusRequestDto;

namespace HireMind.Api.Controllers.HireMindControllers;

public class HiringStagesController : ApiBaseController
{
    private readonly ISender _sender;

    public HiringStagesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("GetAllHiringStagesByJobId/{id}")] 
    public async Task<GetAllHiringStagesByJobIdResult> GetAllCards(int id)
    {
        var result = await _sender.Send(new GetAllHiringStagesByJobIdQuery(id));
        return result;
    }
}
