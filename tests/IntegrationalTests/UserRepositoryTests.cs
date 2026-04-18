using AutoBogus;
using IntegrationalTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Results;
using ObservabilitySample.Service.Application.Models;

namespace IntegrationalTests;

[Collection(nameof(WebApplicationCollectionFixture))]
public sealed class UserRepositoryTests
{
    private readonly WebApplicationFixture _fixture;

    public UserRepositoryTests(WebApplicationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task TryAddAsync_ShouldAddUser()
    {
        // Arrange
        await using AsyncServiceScope scope = _fixture.Services.CreateAsyncScope();
        IUserRepository repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        User? user = new AutoFaker<User>().Generate();

        // Act
        AddUserResult result = await repository.TryAddAsync(user, default);

        // Assert
        result.Should().BeOfType<AddUserResult.Success>();
    }
}
