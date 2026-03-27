using Microsoft.Extensions.Logging;
using ObservabilitySample.Service.Application.Abstractions.Metrics;
using ObservabilitySample.Service.Application.Abstractions.Persistence;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;
using ObservabilitySample.Service.Application.Contracts.Posts;
using ObservabilitySample.Service.Application.Contracts.Posts.Operations;
using ObservabilitySample.Service.Application.Models;
using ObservabilitySample.Service.Application.Specifications;

namespace ObservabilitySample.Service.Application.Posts;

internal sealed class PostService : IPostService
{
    private readonly IPersistenceContext _context;
    private readonly ILogger<PostService> _logger;
    private readonly IServiceMetrics _metrics;

    public PostService(IPersistenceContext context, ILogger<PostService> logger, IServiceMetrics metrics)
    {
        _context = context;
        _logger = logger;
        _metrics = metrics;
    }

    public async Task<CreatePost.Response> CreatePostAsync(
        CreatePost.Request request,
        CancellationToken cancellationToken)
    {
        User? user = await _context.Users
            .FindByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return new CreatePost.Response.UserNotFound();

        var post = new Post(
            Id: default,
            request.UserId,
            request.Title,
            request.Body);

        post = await _context.Posts.AddAsync(post, cancellationToken);

        _logger.LogInformation(
            "Created post '{Title}' with id = '{PostId}'",
            post.Title,
            post.Id);
        
        _metrics.IncPostCreated();

        return new CreatePost.Response.Success(post);
    }

    public async Task<QueryPosts.Response> QueryPostsAsync(
        QueryPosts.Request request,
        CancellationToken cancellationToken)
    {
        var query = PostQuery.Build(builder => builder
            .WithCursorPostId(request.PageToken?.PostId)
            .WithPageSize(request.PageSize)
            .WithUserIds(request.UserIds));

        Post[] posts = await _context.Posts
            .QueryAsync(query, cancellationToken)
            .ToArrayAsync(cancellationToken);

        QueryPosts.PageTokenModel? pageToken = posts.Length < request.PageSize
            ? null
            : new QueryPosts.PageTokenModel(posts[^1].Id);

        _metrics.IncPostsViewed(posts.Length);

        return new QueryPosts.Response(pageToken, posts);
    }
}
