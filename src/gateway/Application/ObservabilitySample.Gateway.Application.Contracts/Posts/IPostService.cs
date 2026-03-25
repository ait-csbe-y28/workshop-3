using ObservabilitySample.Gateway.Application.Contracts.Posts.Operations;
using ObservabilitySample.Gateway.Application.Dto.Posts;

namespace ObservabilitySample.Gateway.Application.Contracts.Posts;

public interface IPostService
{
    Task<PostDto> CreatePostAsync(
        long userId,
        string title,
        string body,
        CancellationToken cancellationToken);

    Task<QueryPosts.Response> QueryPostsAsync(
        QueryPosts.Request request,
        CancellationToken cancellationToken);
}
