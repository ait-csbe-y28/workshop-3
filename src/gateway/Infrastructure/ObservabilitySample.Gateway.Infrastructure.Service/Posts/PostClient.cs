using ObservabilitySample.Gateway.Application.Abstractions.Posts;
using ObservabilitySample.Gateway.Application.Dto.Posts;
using ObservabilitySample.Service.Proto;
using QueryPostsResponse = ObservabilitySample.Gateway.Application.Abstractions.Posts.QueryPostsResponse;

namespace ObservabilitySample.Gateway.Infrastructure.Service.Posts;

internal sealed class PostClient : IPostClient
{
    private readonly PostsService.PostsServiceClient _client;

    public PostClient(PostsService.PostsServiceClient client)
    {
        _client = client;
    }

    public async Task<PostDto> CreatePostAsync(
        long userId,
        string title,
        string body,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new ProtoCreatePostRequest(userId, title, body);

        ProtoCreatePostResponse response = await _client
            .CreatePostAsync(grpcRequest, cancellationToken: cancellationToken);

        return new PostDto(response.PostId, userId, title, body);
    }

    public async Task<QueryPostsResponse> QueryPostsAsync(
        string? pageToken,
        int pageSize,
        IReadOnlyCollection<long> userIds,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new ProtoQueryPostsRequest(pageToken, pageSize, userIds);

        ProtoQueryPostsResponse response = await _client
            .QueryPostsAsync(grpcRequest, cancellationToken: cancellationToken);

        return new QueryPostsResponse(
            response.PageToken,
            response.Posts.Select(post => new PostDto(post.Id, post.UserId, post.Title, post.Body)).ToArray());
    }
}
