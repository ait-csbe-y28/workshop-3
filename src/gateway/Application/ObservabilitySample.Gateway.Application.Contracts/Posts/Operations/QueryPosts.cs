using ObservabilitySample.Gateway.Application.Dto.Posts;

namespace ObservabilitySample.Gateway.Application.Contracts.Posts.Operations;

public static class QueryPosts
{
    public readonly record struct Request(
        string? PageToken,
        int PageSize,
        IReadOnlyCollection<long> UserIds);

    public readonly record struct Response(
        string? PageToken,
        IReadOnlyCollection<PostDto> Posts);
}
