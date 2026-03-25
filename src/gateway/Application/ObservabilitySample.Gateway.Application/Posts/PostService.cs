using Microsoft.Extensions.Caching.Hybrid;
using ObservabilitySample.Gateway.Application.Abstractions.Posts;
using ObservabilitySample.Gateway.Application.Contracts.Posts;
using ObservabilitySample.Gateway.Application.Contracts.Posts.Operations;
using ObservabilitySample.Gateway.Application.Dto.Posts;

namespace ObservabilitySample.Gateway.Application.Posts;

internal sealed class PostService : IPostService
{
    private readonly IPostClient _postClient;
    private readonly HybridCache _cache;

    public PostService(IPostClient postClient, HybridCache cache)
    {
        _postClient = postClient;
        _cache = cache;
    }

    public Task<PostDto> CreatePostAsync(long userId, string title, string body, CancellationToken cancellationToken)
    {
        return _postClient.CreatePostAsync(userId, title, body, cancellationToken);
    }

    public Task<QueryPosts.Response> QueryPostsAsync(QueryPosts.Request request, CancellationToken cancellationToken)
    {
        return request.PageToken is null
            ? GetCachedPostsAsync(request, cancellationToken)
            : GetDirectPostsAsync(request, cancellationToken);
    }

    private async Task<QueryPosts.Response> GetDirectPostsAsync(
        QueryPosts.Request request,
        CancellationToken cancellationToken)
    {
        QueryPostsResponse response = await _postClient
            .QueryPostsAsync(request.PageToken, request.PageSize, request.UserIds, cancellationToken);

        return new QueryPosts.Response(response.PageToken, response.Posts);
    }

    private async Task<QueryPosts.Response> GetCachedPostsAsync(
        QueryPosts.Request request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"{request.PageSize}:{string.Join("|", request.UserIds)}";

        return await _cache.GetOrCreateAsync(
            cacheKey,
            (request, service: this),
            static async (state, ct) => await state.service.GetDirectPostsAsync(state.request, ct),
            cancellationToken: cancellationToken);
    }
}
