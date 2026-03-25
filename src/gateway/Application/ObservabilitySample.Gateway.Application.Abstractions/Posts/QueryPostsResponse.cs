using ObservabilitySample.Gateway.Application.Dto.Posts;

namespace ObservabilitySample.Gateway.Application.Abstractions.Posts;

public readonly record struct QueryPostsResponse(
    string? PageToken,
    IReadOnlyCollection<PostDto> Posts);
