using AutoBogus;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ObservabilitySample.Service.Application.Abstractions.Metrics;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;
using ObservabilitySample.Service.Application.Contracts.Posts.Operations;
using ObservabilitySample.Service.Application.Models;
using ObservabilitySample.Service.Application.Posts;
using UnitTests.Mocks;

namespace UnitTests;

public sealed class PostServiceTests : IAsyncLifetime
{
    private readonly MockPersistenceContext _persistenceContext = new();
    private readonly Mock<IServiceMetrics> _serviceMetrics = new(MockBehavior.Strict);

    private readonly PostService _postService;

    private readonly Faker _faker = new();

    public PostServiceTests()
    {
        _postService = new PostService(
            _persistenceContext,
            NullLogger<PostService>.Instance,
            _serviceMetrics.Object);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _persistenceContext.VerifyAll();
        _serviceMetrics.VerifyAll();

        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreatePostAsync_ShouldSucceed_WhenUserExists()
    {
        // Arrange
        var expectedPostId = 1;
        User user = new AutoFaker<User>().Generate();

        _persistenceContext.Users
            .Setup(repo => repo.QueryAsync(
                It.Is<UserQuery>(q => q.UserIds.Contains(user.Id)),
                It.IsAny<CancellationToken>()))
            .Returns(new[] { user }.ToAsyncEnumerable);

        var request = new CreatePost.Request(
            user.Id,
            _faker.Commerce.Department(),
            _faker.Commerce.ProductDescription());

        var expectedPost = new Post(
            default,
            user.Id,
            request.Title,
            request.Body);

        _persistenceContext.Posts
            .Setup(repo => repo.AddAsync(expectedPost, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPost with { Id = expectedPostId });

        _serviceMetrics.Setup(m => m.IncPostCreated());

        // Act
        CreatePost.Response response = await _postService.CreatePostAsync(request, default);

        // Assert
        response
            .Should()
            .BeOfType<CreatePost.Response.Success>()
            .Which.Post.Should()
            .BeEquivalentTo(expectedPost with { Id = expectedPostId });
    }

    [Theory]
    [InlineData(10, 10, true)]
    [InlineData(10, 9, false)]
    [InlineData(10, 11, true)]
    // MemberData
    // ClassData
    // TheoryData<>
    public async Task QueryPostsAsync_ShouldReturnExpectedResult(
        int requestPageSize,
        int postCount,
        bool pageTokenReturned)
    {
        // Arrange
        List<Post> posts = new AutoFaker<Post>().Generate(postCount);

        var expectedQuery = PostQuery.Build(builder => builder.WithPageSize(requestPageSize));

        _persistenceContext.Posts
            .Setup(repo => repo.QueryAsync(
                expectedQuery,
                It.IsAny<CancellationToken>()))
            .Returns(posts.ToAsyncEnumerable);

        _serviceMetrics.Setup(m => m.IncPostsViewed(It.IsAny<int>()));

        // Act
        QueryPosts.Response response = await _postService.QueryPostsAsync(
            new QueryPosts.Request(null, requestPageSize, []),
            default);

        // Assert
        response.Posts.Should().HaveCount(posts.Count);

        if (pageTokenReturned)
        {
            response.PageToken.Should().NotBeNull();
        }
        else
        {
            response.PageToken.Should().BeNull();
        }
    }
}
