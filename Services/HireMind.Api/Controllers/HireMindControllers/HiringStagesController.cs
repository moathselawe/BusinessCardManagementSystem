using HireMind.Application.Queries.HiringStages;
using Microsoft.AspNetCore.Authorization;

namespace HireMind.Api.Controllers.HireMindControllers;
[Authorize]

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
