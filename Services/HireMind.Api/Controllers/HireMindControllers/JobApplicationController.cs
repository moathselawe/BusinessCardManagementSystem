using HireMind.Application.Queries.JobApplication;

namespace HireMind.Api.Controllers.HireMindControllers;

public class JobApplicationController : ApiBaseController
{
    private readonly ISender _sender;

    public JobApplicationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("analyze")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AnalyzeCvResult>> Analyze([FromForm] AnalyzeCvRequestDto request)
    {
        try
        {
            var result = await _sender.Send(new AnalyzeCvCommand(request));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitJobApplicationRequestDto request)
    {
        var result = await _sender.Send(new SubmitJobApplicationCommand(request));

        return Ok(result.Id);
    }


    [HttpGet("GetAllByJobId/{jobId}")]
    public async Task<GetAllJobApplicationsByJobIdResult> GetAllJobApplicationsByJobId(int jobId)
    {
        var result = await _sender.Send(new GetAllJobApplicationsByJobIdQuery(jobId));
        return result;
    }
}
