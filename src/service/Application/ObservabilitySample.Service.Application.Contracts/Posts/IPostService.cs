using ObservabilitySample.Service.Application.Contracts.Posts.Operations;

namespace ObservabilitySample.Service.Application.Contracts.Posts;

public interface IPostService
{
    Task<CreatePost.Response> CreatePostAsync(
        CreatePost.Request request,
        CancellationToken cancellationToken);

    Task<QueryPosts.Response> QueryPostsAsync(
        QueryPosts.Request request,
        CancellationToken cancellationToken);
}
