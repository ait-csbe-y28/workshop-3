using ObservabilitySample.Gateway.Application.Dto.Users;

namespace ObservabilitySample.Gateway.Application.Abstractions.Users;

public interface IUserClient
{
    Task<UserDto> CreateUserAsync(
        string login,
        string name,
        CancellationToken cancellationToken);
}
