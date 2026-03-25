using ObservabilitySample.Gateway.Application.Dto.Users;

namespace ObservabilitySample.Gateway.Application.Contracts.Users;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(
        string login,
        string name,
        CancellationToken cancellationToken);
}
