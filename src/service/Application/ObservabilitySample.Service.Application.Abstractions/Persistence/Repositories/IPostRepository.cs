using ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;
using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;

public interface IPostRepository
{
    IAsyncEnumerable<Post> QueryAsync(
        PostQuery query,
        CancellationToken cancellationToken);

    Task<Post> AddAsync(
        Post post,
        CancellationToken cancellationToken);
}
