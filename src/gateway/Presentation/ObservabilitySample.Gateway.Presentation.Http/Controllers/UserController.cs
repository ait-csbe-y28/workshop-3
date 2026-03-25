using Microsoft.AspNetCore.Mvc;
using ObservabilitySample.Gateway.Application.Contracts.Users;
using ObservabilitySample.Gateway.Application.Dto.Users;
using ObservabilitySample.Gateway.Presentation.Http.Models;

namespace ObservabilitySample.Gateway.Presentation.Http.Controllers;

[ApiController]
[Route("/api/user")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> CreateUserAsync(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        UserDto user = await _userService.CreateUserAsync(
            request.Login,
            request.Name,
            cancellationToken);

        return Ok(new CreateUserResponse
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
        });
    }
}
