namespace ObservabilitySample.Gateway.Presentation.Http.Models.Posts;

public sealed class CreatePostResponse
{
    public required long Id { get; init; }

    public required long UserId { get; init; }

    public required string Title { get; init; }

    public required string Body { get; init; }
}
