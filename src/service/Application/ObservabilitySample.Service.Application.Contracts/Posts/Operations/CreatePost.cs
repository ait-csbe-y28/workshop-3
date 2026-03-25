using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Contracts.Posts.Operations;

public static class CreatePost
{
    public readonly record struct Request(long UserId, string Title, string Body);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(Post Post) : Response;

        public sealed record UserNotFound : Response;
    }
}
