using System.Diagnostics;
using ObservabilitySample.Service.Application.Abstractions.Persistence;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Results;
using ObservabilitySample.Service.Application.Contracts.Users;
using ObservabilitySample.Service.Application.Contracts.Users.Operations;
using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Users;

internal sealed class UserService : IUserService
{
    private readonly IPersistenceContext _context;

    public UserService(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<CreateUser.Response> CreateUserAsync(
        CreateUser.Request request,
        CancellationToken cancellationToken)
    {
        var user = new User(Id: default, request.Login, request.Name);

        AddUserResult result = await _context.Users.TryAddAsync(
            user,
            cancellationToken);

        return result switch
        {
            AddUserResult.Success success => new CreateUser.Response.Success(success.User),
            AddUserResult.LoginConflict => new CreateUser.Response.LoginConflict(),

            _ => throw new UnreachableException(),
        };
    }
}
