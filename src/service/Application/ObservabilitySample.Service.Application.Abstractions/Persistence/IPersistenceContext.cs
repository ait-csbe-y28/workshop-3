using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;

namespace ObservabilitySample.Service.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IUserRepository Users { get; }
    
    IPostRepository Posts { get; }
}
