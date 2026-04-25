namespace HireMind.Api.Controllers.ContentController;

public class AboutUsController : ApiBaseController
{
    private readonly ISender _sender;

    public AboutUsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("GetALL")]
    public async Task<GetAllAboutUsResult> GetAll()
    {
        var result = await _sender.Send(new GetAllAboutUsQuery());
        return result;
    }

}
