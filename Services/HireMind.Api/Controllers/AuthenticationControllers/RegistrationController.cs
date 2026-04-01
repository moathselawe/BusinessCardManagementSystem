using HireMind.Domain.Dtos.Authentication;

namespace HireMind.Api.Controllers.AuthenticationControllers;

public class RegistrationController : ApiBaseController
{
    private readonly ISender _sender;

    public RegistrationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRqDto command)
    {
        var result = await _sender.Send(new RegisterUserCommand(command));

        if (result.Id != null)
            return Ok(result.Id);
        else
            return BadRequest("Error while Register User");
    }

    [HttpGet("verify-email/{token}")]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        var result = await _sender.Send(new VerifyEmailCommand(token));

        return Ok(result);
    }

    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerificationEmail(
        [FromBody] ResendVerificationEmailRequest request)
    {
        var result = await _sender.Send(
            new ResendVerificationEmailCommand(request.Email));

        return Ok(result);
    }
}
