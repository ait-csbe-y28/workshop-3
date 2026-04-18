using Bogus;
using FluentAssertions.Specialized;
using Grpc.Net.Client;
using IntegrationalTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;
using ObservabilitySample.Service.Application.Models;
using ObservabilitySample.Service.Application.Specifications;
using ObservabilitySample.Service.Proto;

namespace IntegrationalTests;

[Collection(nameof(WebApplicationCollectionFixture))]
public sealed class UserControllerTests
{
    private readonly WebApplicationFixture _fixture;
    private readonly UserService.UserServiceClient _userServiceClient;

    private readonly Faker _faker = new Faker
    {
        Random = new Randomizer(42),
    };

    public UserControllerTests(WebApplicationFixture fixture)
    {
        _fixture = fixture;
        _userServiceClient = new UserService.UserServiceClient(fixture.CreateChannel());
    }

    [Fact]
    public async Task CreateUser_ShouldCreateUser()
    {
        // Arrange
        var request = new CreateUserRequest(
            _faker.Internet.UserName(),
            _faker.Person.FirstName);

        // Act
        Func<Task<CreateUserResponse>> responseFunc = async () => await _userServiceClient.CreateUserAsync(
            request,
            cancellationToken: default);

        // Assert
        var response = await responseFunc.Should().NotThrowAsync();
        response.Subject.Login.Should().Be(request.Login);
        response.Subject.Name.Should().Be(request.Name);

        await using AsyncServiceScope scope = _fixture.Services.CreateAsyncScope();
        IUserRepository repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        User? user = await repository.FindByIdAsync(response.Subject.UserId, default);
        user.Should().NotBeNull();
    }
}
