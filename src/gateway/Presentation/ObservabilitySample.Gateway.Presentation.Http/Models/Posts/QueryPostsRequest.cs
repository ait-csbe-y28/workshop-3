using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ObservabilitySample.Gateway.Presentation.Http.Models.Posts;

public sealed class QueryPostsRequest
{
    [FromQuery(Name = "pageToken")]
    public string? PageToken { get; init; }

    [FromQuery(Name = "pageSize")]
    [Range(minimum: 1, maximum: 100)]
    public required int PageSize { get; init; }

    [MinLength(1)]
    public required long[] UserIds { get; init; }
}
