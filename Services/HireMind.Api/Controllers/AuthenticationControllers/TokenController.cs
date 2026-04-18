namespace HireMind.Api.Controllers.AuthenticationControllers;

[AllowAnonymous]
public class TokenController : ApiBaseController
{
    private readonly ISender _sender;

    public TokenController(ISender sender)
    {
        _sender = sender; 
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _sender.Send(command);

        if (!result.Response.IsSuccess)
            return BadRequest(result.Response);

        return Ok(result.Response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRqDto request)
    {
        var result = await _sender.Send(new RefreshTokenCommand(request));

        if (!result.Response.IsSuccess)
            return BadRequest(result.Response);

        return Ok(result.Response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRqDto request)
    {
        var result = await _sender.Send(new LogoutCommand(request));

        if (!result.IsSuccess)
            return BadRequest(new { result.IsSuccess, result.Message });

        return Ok(new { result.IsSuccess, result.Message });
    }

    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll([FromBody] LogoutFromAllDevicesRqDto request)
    {
        var result = await _sender.Send(new LogoutFromAllDevicesCommand(request));

        if (!result.IsSuccess)
            return BadRequest(new { result.IsSuccess, result.Message });

        return Ok(new { result.IsSuccess, result.Message });
    }
}
