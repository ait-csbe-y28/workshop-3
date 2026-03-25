using ObservabilitySample.Gateway.Application.Abstractions.Users;
using ObservabilitySample.Gateway.Application.Contracts.Users;
using ObservabilitySample.Gateway.Application.Dto.Users;

namespace ObservabilitySample.Gateway.Application.Users;

internal sealed class UserService : IUserService
{
    private readonly IUserClient _userClient;

    public UserService(IUserClient userClient)
    {
        _userClient = userClient;
    }

    public Task<UserDto> CreateUserAsync(string login, string name, CancellationToken cancellationToken)
    {
        return _userClient.CreateUserAsync(login, name, cancellationToken);
    }
}
