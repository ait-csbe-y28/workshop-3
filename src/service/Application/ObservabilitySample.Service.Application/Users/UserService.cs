using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ObservabilitySample.Service.Application.Abstractions.Metrics;
using ObservabilitySample.Service.Application.Abstractions.Persistence;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Results;
using ObservabilitySample.Service.Application.Contracts.Users;
using ObservabilitySample.Service.Application.Contracts.Users.Operations;
using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Users;

internal sealed class UserService : IUserService
{
    private readonly IPersistenceContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly IServiceMetrics _metrics;

    public UserService(IPersistenceContext context, ILogger<UserService> logger, IServiceMetrics metrics)
    {
        _context = context;
        _logger = logger;
        _metrics = metrics;
    }

    public async Task<CreateUser.Response> CreateUserAsync(
        CreateUser.Request request,
        CancellationToken cancellationToken)
    {
        var user = new User(Id: default, request.Login, request.Name);

        AddUserResult result = await _context.Users.TryAddAsync(
            user,
            cancellationToken);

        if (result is AddUserResult.Success success)
        {
            _logger.LogInformation(
                "Successfully created user = '{Login}', with id = '{UserId}'",
                success.User.Login,
                success.User.Id);
            
            _metrics.IncUserCreated();

            return new CreateUser.Response.Success(success.User);
        }

        if (result is AddUserResult.LoginConflict)
        {
            _logger.LogInformation(
                "Failed to create user with login = '{Login}' due to login conflict",
                user.Login);

            return new CreateUser.Response.LoginConflict();
        }

        throw new UnreachableException();
    }
}
