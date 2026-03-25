namespace ObservabilitySample.Service.Application.Models;

public sealed record Post(
    long Id,
    long UserId,
    string Title,
    string Body);
