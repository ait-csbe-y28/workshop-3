using ObservabilitySample.Gateway.Application.Dto.Posts;

namespace ObservabilitySample.Gateway.Application.Abstractions.Posts;

public interface IPostClient
{
    Task<PostDto> CreatePostAsync(
        long userId,
        string title,
        string body,
        CancellationToken cancellationToken);

    Task<QueryPostsResponse> QueryPostsAsync(
        string? pageToken,
        int pageSize,
        IReadOnlyCollection<long> userIds,
        CancellationToken cancellationToken);
}
