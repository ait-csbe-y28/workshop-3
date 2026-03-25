using ObservabilitySample.Gateway.Application.Dto.Posts;

namespace ObservabilitySample.Gateway.Presentation.Http.Models.Posts;

public sealed class QueryPostsResponse
{
    public required string? PageToken { get; init; }

    public required IReadOnlyCollection<PostDto> Posts { get; init; }
}
