using HireMind.Domain.SeedWork;
using System.Threading;

namespace HireMind.Api.Controllers.HireMindControllers;

//[Authorize]
public class ManageUsersController : ApiBaseController
{
    private readonly ISender _sender;
    private readonly IUserRepository _repository;

    public ManageUsersController(ISender sender, IUserRepository repository)
    {
        _sender = sender;
        _repository = repository;
    }

    [HttpGet("getAll")]
    public async Task<GetAllUsersResult> GetAll()
    {
        var result = await _sender.Send(new GetAllUsersQuery());
        return result;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFiltersRqDto filters)
    {
        var result = await _sender.Send(new SearchUsersQuery(filters));
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<GetUserByIdResult> GetUserById(Guid id)
    {
        var result = await _sender.Send(new GetUserByIdQuery(id));

        return result;
    }

    [HttpPost("create")]
    public async Task<IActionResult> create([FromBody] CreateUserByAdminRqDto command)
    {
        var result = await _sender.Send(new CreateUserByAdminCommand(command));

        if (result.UserId != null)
            return Ok(new
            {
                userId = result.UserId,
                success = true
            });
        else
            return BadRequest("Failed to create User.");
    }

    [HttpPut("update")]
    public async Task<IActionResult> update([FromBody] UpdateUserRequestDto command)
    {
        var result = await _sender.Send(new UpdateUserCommand(command));

        if (result.IsSuccess)
            return Ok(command.Id);
        else
            return BadRequest("Failed to update User.");
    }

    [HttpPut("update/lockStatus")]
    public async Task<IActionResult> updateLockStatus([FromBody] UpdateUserLockStatusRequestDto command)
    {
        var result = await _sender.Send(new UpdateUserLockStatusCommand(command));

        if (result.IsSuccess)
            return Ok(command.Id);
        else
            return BadRequest("Failed to update User.");
    }

    [HttpPut("update/userRoles")]
    public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesRqDto dto)
    {
        var result = await _sender.Send(new UpdateUserRolesCommand(dto));

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest("Failed to update role.");
    }


    //[HttpDelete("delete/{id}")]
    //public async Task<IActionResult> delete(Guid id)
    //{
    //    var result = await _sender.Send(new DeleteUserCommand(id));

    //    if (result.IsSuccess)
    //        return Ok(new { message = $"User {id} deleted successfully." });

    //    return NotFound(new { message = $"User {id} not found." });
    //}
}
