using System.Diagnostics;
using Grpc.Core;
using Itmo.Dev.Platform.Common.Serialization;
using ObservabilitySample.Service.Application.Contracts.Posts;
using ObservabilitySample.Service.Application.Contracts.Posts.Operations;
using ObservabilitySample.Service.Proto;

namespace ObservabilitySample.Service.Presentation.Grpc.Controllers;

public sealed class PostController : PostsService.PostsServiceBase
{
    private readonly IPostService _postService;
    private readonly IPlatformSerializer _serializer;

    public PostController(IPostService postService, IPlatformSerializer serializer)
    {
        _postService = postService;
        _serializer = serializer;
    }

    public override async Task<ProtoCreatePostResponse> CreatePost(
        ProtoCreatePostRequest request,
        ServerCallContext context)
    {
        var applicationRequest = new CreatePost.Request(
            request.UserId,
            request.Title,
            request.Body);

        CreatePost.Response response = await _postService
            .CreatePostAsync(applicationRequest, context.CancellationToken);

        return response switch
        {
            CreatePost.Response.Success success => new ProtoCreatePostResponse(success.Post.Id),

            CreatePost.Response.UserNotFound _ => throw new RpcException(
                new Status(StatusCode.NotFound, "User not found")),

            _ => throw new UnreachableException(),
        };
    }

    public override async Task<ProtoQueryPostsResponse> QueryPosts(
        ProtoQueryPostsRequest request,
        ServerCallContext context)
    {
        QueryPosts.PageTokenModel? requestPageToken = request.PageToken is null
            ? null
            : _serializer.Deserialize<QueryPosts.PageTokenModel>(request.PageToken);

        var applicationRequest = new QueryPosts.Request(
            requestPageToken,
            request.PageSize,
            request.UserIds);

        QueryPosts.Response response = await _postService
            .QueryPostsAsync(applicationRequest, context.CancellationToken);

        string? responsePageToken = response.PageToken is null
            ? null
            : _serializer.Serialize(response.PageToken.Value);

        return new ProtoQueryPostsResponse(
            responsePageToken,
            response.Posts.Select(post => new ProtoQueryPostsResponse.Types.Post(
                post.Id,
                post.UserId,
                post.Title,
                post.Body)));
    }
}
