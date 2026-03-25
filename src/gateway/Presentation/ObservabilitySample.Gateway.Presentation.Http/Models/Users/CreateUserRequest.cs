using System.ComponentModel.DataAnnotations;

namespace ObservabilitySample.Gateway.Presentation.Http.Models;

public sealed class CreateUserRequest
{
    [MinLength(1)]
    [RegularExpression("[a-zA-Z0-9_]+")]
    public required string Login { get; init; }

    [MinLength(1)]
    public required string Name { get; init; }
}
