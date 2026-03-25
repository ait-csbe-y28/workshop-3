using System.Diagnostics;
using Grpc.Core;
using ObservabilitySample.Service.Application.Contracts.Users;
using ObservabilitySample.Service.Application.Contracts.Users.Operations;
using ObservabilitySample.Service.Proto;

namespace ObservabilitySample.Service.Presentation.Grpc.Controllers;

public sealed class UserController : UserService.UserServiceBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<ProtoCreateUserResponse> CreateUser(
        ProtoCreateUserRequest request,
        ServerCallContext context)
    {
        var applicationRequest = new CreateUser.Request(
            request.Login,
            request.Name);

        CreateUser.Response response = await _userService
            .CreateUserAsync(applicationRequest, context.CancellationToken);

        return response switch
        {
            CreateUser.Response.Success success => new ProtoCreateUserResponse(
                success.User.Id,
                success.User.Login,
                success.User.Name),

            CreateUser.Response.LoginConflict _ => throw new RpcException(
                new Status(StatusCode.AlreadyExists, "User with specified login already exists")),

            _ => throw new UnreachableException(),
        };
    }
}
