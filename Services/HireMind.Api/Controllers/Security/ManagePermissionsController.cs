namespace HireMind.Api.Controllers.HireMindControllers;

//[Authorize]
public class ManagePermissionsController : ApiBaseController
{
    private readonly ISender _sender;

    public ManagePermissionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFiltersRqDto filters)
    {
        var result = await _sender.Send(new SearchPermissionsQuery(filters));
        return Ok(result);
    }

    [HttpGet("getAll")]
    public async Task<GetAllPermissionsResult> GetAllCards()
    {
        var result = await _sender.Send(new GetAllPermissionsQuery());
        return result;
    }

}
