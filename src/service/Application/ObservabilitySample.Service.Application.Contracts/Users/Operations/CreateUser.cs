using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Contracts.Users.Operations;

public static class CreateUser
{
    public readonly record struct Request(string Login, string Name);

    public abstract record Response
    {
        private Response() { }

        public sealed record Success(User User) : Response;

        public sealed record LoginConflict : Response;
    }
}
