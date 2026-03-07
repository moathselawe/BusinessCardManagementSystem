namespace BCMS.Api.Controllers.HireMindControllers;

public class JobApplicationController : ApiBaseController
{
    private readonly ISender _sender;

    public JobApplicationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("analyze")]
    [Consumes("multipart/form-data")]
    public async Task<AnalyzeCvResult> Analyze([FromForm] AnalyzeCvRequestDto request)
    {
        var result = await _sender.Send(new AnalyzeCvCommand(request));
        return result;
    }

    //[HttpPost("submit")]
    //public async Task<JobApplicationResponse> Submit([FromBody] SubmitJobApplicationRequest request)
    //{
    //    return await _sender.Send(new SubmitJobApplicationCommand(request));
    //}
}
