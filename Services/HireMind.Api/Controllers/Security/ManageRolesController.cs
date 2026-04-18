using HireMind.Application.Queries.Security;

namespace HireMind.Api.Controllers.HireMindControllers;

//[Authorize]
public class ManageRolesController : ApiBaseController
{
    private readonly ISender _sender;

    public ManageRolesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _sender.Send(new GetAllRolesQuery());

        return Ok(result);
    }


    [HttpGet("getAll")]
    public async Task<GetAllRolesResult> GetAll()
    {
        var result = await _sender.Send(new GetAllRolesQuery());
        return result;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchFiltersRqDto filters)
    {
        var result = await _sender.Send(new SearchRolesQuery(filters));
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] RoleRqDto dto)
    {
        var result = await _sender.Send(new CreateRoleCommand(dto));

        if (!string.IsNullOrEmpty(result.RoleId))
            return Ok(result);

        return BadRequest("Failed to create role.");
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateRoleRqDto dto)
    {
        var result = await _sender.Send(new UpdateRoleCommand(dto));

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest("Failed to update role.");
    }

    [HttpPut("update/rolePermisiions")]
    public async Task<IActionResult> UpdateRolePermisiions([FromBody] UpdateRolePermissionsRqDto dto)
    {
        var result = await _sender.Send(new UpdateRolePermissionsCommand(dto));

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest("Failed to update role.");
    }


    //[HttpDelete("delete/{id}")]
    //public async Task<IActionResult> delete(Guid id)
    //{
    //    var result = await _sender.Send(new DeleteRoleCommand(id));

    //    if (result.IsSuccess)
    //        return Ok(new { message = $"Role {id} deleted successfully." });

    //    return NotFound(new { message = $"Role {id} not found." });
    //}

    [HttpGet("get/{id}")]
    public async Task<GetRoleByIdResult> GetRoleById(Guid id)
    {
        var result = await _sender.Send(new GetRoleByIdQuery(id));

        return result;
    }
}
