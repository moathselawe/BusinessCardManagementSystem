using HireMind.Application.Queries.JobApplication;
using Microsoft.AspNetCore.Authorization;

namespace HireMind.Api.Controllers.HireMindControllers;
[Authorize]

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

    [HttpGet("getJobApplicationById/{id}")]
    public async Task<GetJobApplicationByIdResult> getJobApplicationById(int id)
    {
        var result = await _sender.Send(new GetJobApplicationByIdQuery(id));
        return result;
    }

    [HttpGet("download-cv/{applicationId}")]
    public async Task<IActionResult> DownloadCv(int applicationId)
    {
        // 1. Get CV path from database
        var result = await _sender.Send(new GetCvByApplicationIdQuery(applicationId));

        if (string.IsNullOrEmpty(result.CvFilePath))
            return NotFound("CV not found.");

        var fullPath = Path.Combine("wwwroot", result.CvFilePath);

        if (!System.IO.File.Exists(fullPath))
            return NotFound("CV file missing.");

        var fileName = Path.GetFileName(fullPath);

        // 2. Determine MIME type dynamically
        var extension = Path.GetExtension(fileName).ToLower();
        string contentType = extension switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };

        // 3. Return file as stream
        var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return File(fileStream, contentType, fileName);
    }
}

