using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ObservabilitySample.Gateway.Application.Contracts.Posts;
using ObservabilitySample.Gateway.Application.Contracts.Posts.Operations;
using ObservabilitySample.Gateway.Application.Dto.Posts;
using ObservabilitySample.Gateway.Presentation.Http.Extensions;
using ObservabilitySample.Gateway.Presentation.Http.Models.Posts;

namespace ObservabilitySample.Gateway.Presentation.Http.Controllers;

[ApiController]
[Route("/api/post")]
public sealed class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<ActionResult<CreatePostResponse>> CreatePostAsync(
        [FromBody] CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        Activity.Current.AddUserIdBaggage(request.UserId);
        
        PostDto post = await _postService.CreatePostAsync(
            request.UserId,
            request.Title,
            request.Body,
            cancellationToken);

        return Ok(new CreatePostResponse
        {
            Id = post.Id,
            UserId = post.UserId,
            Title = post.Title,
            Body = post.Body,
        });
    }

    [HttpGet]
    public async Task<ActionResult<QueryPostsResponse>> QueryPostsAsync(
        [FromQuery] QueryPostsRequest request,
        CancellationToken cancellationToken)
    {
        var applicationRequest = new QueryPosts.Request(
            request.PageToken,
            request.PageSize,
            request.UserIds);

        QueryPosts.Response response = await _postService
            .QueryPostsAsync(applicationRequest, cancellationToken);

        return Ok(new QueryPostsResponse
        {
            PageToken = response.PageToken,
            Posts = response.Posts,
        });
    }
}
