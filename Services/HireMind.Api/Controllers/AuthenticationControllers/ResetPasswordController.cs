namespace HireMind.Api.Controllers.AuthenticationControllers;

[AllowAnonymous]
public class ResetPasswordController : ApiBaseController
{
    private readonly ISender _sender;

    public ResetPasswordController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendResetCode([FromBody] SendResetCodeRequest request)
    {
        var result = await _sender.Send(new SendResetCodeCommand(request.Email));

        return Ok(result);
    }

    [HttpPost("verify-code")]
    public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeRequest request)
    {
        var result = await _sender.Send(new VerifyResetCodeCommand(request.Email, request.Otp));

        return Ok(result);
    }

    [HttpPost("save-password")]
    public async Task<IActionResult> SaveNewPassword([FromBody] SaveNewPasswordRqDto request)
    {
        var result = await _sender.Send(new SaveNewPasswordCommand(request));

        return Ok(result);
    }


}
