using System.ComponentModel.DataAnnotations;

namespace ObservabilitySample.Gateway.Presentation.Http.Models.Posts;

public sealed class CreatePostRequest
{
    [Range(minimum: 1, maximum: long.MaxValue)]
    public required long UserId { get; init; }
    
    [MinLength(1)]
    public required string Title { get; init; }
    
    [MinLength(1)]
    public required string Body { get; init; }
}
