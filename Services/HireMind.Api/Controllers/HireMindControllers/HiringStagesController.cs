namespace HireMind.Api.Controllers.HireMindControllers;

[Authorize]
public class HiringStagesController : ApiBaseController
{
    private readonly ISender _sender;

    public HiringStagesController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Policy = PermissionConstants.HiringStages.View)]
    [HttpGet("GetAllHiringStagesByJobId/{id}")] 
    public async Task<GetAllHiringStagesByJobIdResult> GetAllCards(int id)
    {
        var result = await _sender.Send(new GetAllHiringStagesByJobIdQuery(id));
        return result;
    }
}
