using ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Results;
using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;

public interface IUserRepository
{
    IAsyncEnumerable<User> QueryAsync(
        UserQuery query,
        CancellationToken cancellationToken);

    Task<AddUserResult> TryAddAsync(
        User user,
        CancellationToken cancellationToken);
}
