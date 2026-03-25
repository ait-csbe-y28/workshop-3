using ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;
using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Specifications;

public static class UserSpecifications
{
    public static ValueTask<User?> FindByIdAsync(
        this IUserRepository repository,
        long userId,
        CancellationToken cancellationToken)
    {
        var query = UserQuery.Build(builder => builder.WithUserId(userId));
        return repository.QueryAsync(query, cancellationToken).SingleOrDefaultAsync(cancellationToken);
    }
}
