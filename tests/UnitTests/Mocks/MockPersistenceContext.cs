using Moq;
using ObservabilitySample.Service.Application.Abstractions.Persistence;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;

namespace UnitTests.Mocks;

public sealed class MockPersistenceContext : IPersistenceContext
{
    public Mock<IUserRepository> Users { get; } = new(MockBehavior.Strict);
    public Mock<IPostRepository> Posts { get; } = new(MockBehavior.Strict);

    IUserRepository IPersistenceContext.Users => Users.Object;
    IPostRepository IPersistenceContext.Posts => Posts.Object;

    public void VerifyAll()
    {
        Users.VerifyAll();
        Posts.VerifyAll();
    }
}
