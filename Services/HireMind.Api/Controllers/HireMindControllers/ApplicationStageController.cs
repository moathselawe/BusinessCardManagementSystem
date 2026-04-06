using HireMind.Application.Commands.ApplicationStage;
using HireMind.Application.Queries.ApplicationStage;
using HireMind.Domain.Dtos.ApplicationStage;
using HireMind.Domain.Dtos.UpdateApplicationStageStatusRequestDto;
using Microsoft.AspNetCore.Authorization;

namespace HireMind.Api.Controllers.HireMindControllers;
[Authorize]

public class ApplicationStageController : ApiBaseController
{
    private readonly ISender _sender;

    public ApplicationStageController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPut("updateBulkApplicationsStageStatus")]
    public async Task<IActionResult> updateBulkApplicationsStageStatus([FromBody] UpdateBulkApplicationsStageStatusRequestDto command)
    {
        var result = await _sender.Send(new UpdateBulkApplicationsStageStatusCommand(command));

        if (result.IsSuccess)
            return Ok(command.Ids);
        else
            return BadRequest("Failed to Update Application Stage Status for selected applications.");
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchJobApplicationsRequestDto filters)
    {
        var result = await _sender.Send(new SearchJobApplicationQuery(filters));
        return Ok(result);
    }

}
