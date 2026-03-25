using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Application.Abstractions.Persistence.Results;

public abstract record AddUserResult
{
    private AddUserResult() { }

    public sealed record Success(User User) : AddUserResult;

    public sealed record LoginConflict : AddUserResult;
}
