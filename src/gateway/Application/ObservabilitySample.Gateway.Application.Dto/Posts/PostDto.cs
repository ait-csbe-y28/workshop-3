namespace ObservabilitySample.Gateway.Application.Dto.Posts;

public sealed record PostDto(long Id, long UserId, string Title, string Body);
