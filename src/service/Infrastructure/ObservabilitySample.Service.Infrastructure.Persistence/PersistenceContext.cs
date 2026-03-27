using ObservabilitySample.Service.Application.Abstractions.Persistence;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;

namespace ObservabilitySample.Service.Infrastructure.Persistence;

internal sealed class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(IUserRepository users, IPostRepository posts)
    {
        Users = users;
        Posts = posts;
    }

    public IUserRepository Users { get; }
    public IPostRepository Posts { get; }
}
