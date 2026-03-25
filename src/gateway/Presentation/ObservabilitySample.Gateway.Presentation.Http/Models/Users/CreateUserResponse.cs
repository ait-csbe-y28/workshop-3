namespace ObservabilitySample.Gateway.Presentation.Http.Models;

public sealed class CreateUserResponse
{
    public required long Id { get; init; }
    
    public required string Login { get; init; }
    
    public required string Name { get; init; }
}
