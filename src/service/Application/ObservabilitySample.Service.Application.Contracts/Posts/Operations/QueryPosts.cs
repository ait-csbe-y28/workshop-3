using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Contracts.Posts.Operations;

public static class QueryPosts
{
    public readonly record struct PageTokenModel(long PostId);

    public readonly record struct Request(
        PageTokenModel? PageToken,
        int PageSize,
        IReadOnlyList<long> UserIds);

    public readonly record struct Response(
        PageTokenModel? PageToken,
        Post[] Posts);
}
