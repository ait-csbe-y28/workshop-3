using ObservabilitySample.Service.Application.Contracts.Users.Operations;

namespace ObservabilitySample.Service.Application.Contracts.Users;

public interface IUserService
{
    Task<CreateUser.Response> CreateUserAsync(
        CreateUser.Request request,
        CancellationToken cancellationToken);
}
