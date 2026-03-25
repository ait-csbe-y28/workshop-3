using ObservabilitySample.Gateway.Application.Abstractions.Users;
using ObservabilitySample.Gateway.Application.Dto.Users;
using ObservabilitySample.Service.Proto;

namespace ObservabilitySample.Gateway.Infrastructure.Service.Users;

internal sealed class UserClient : IUserClient
{
    private readonly UserService.UserServiceClient _client;

    public UserClient(UserService.UserServiceClient client)
    {
        _client = client;
    }

    public async Task<UserDto> CreateUserAsync(
        string login,
        string name,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new ProtoCreateUserRequest(login, name);

        ProtoCreateUserResponse response = await _client
            .CreateUserAsync(grpcRequest, cancellationToken: cancellationToken);

        return new UserDto(response.UserId, response.Login, response.Name);
    }
}
