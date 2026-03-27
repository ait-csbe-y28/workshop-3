using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;
using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Infrastructure.Persistence.Repositories;

public sealed class PostRepository : IPostRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public PostRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async IAsyncEnumerable<Post> QueryAsync(
        PostQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT  post_id,
                user_id,
                post_title,
                post_body
        FROM posts
        WHERE 
            (:cursor_post_id is null OR post_id > :cursor_post_id)
            AND (cardinality(:user_ids) = 0 OR user_id = ANY(:user_ids))
        LIMIT :page_size;
        """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("cursor_post_id", query.CursorPostId)
            .AddParameter("page_size", query.PageSize)
            .AddParameter("user_ids", query.UserIds);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Post(
                Id: reader.GetInt64("post_id"),
                UserId: reader.GetInt64("user_id"),
                Title: reader.GetString("post_title"),
                Body: reader.GetString("post_body"));
        }
    }

    public async Task<Post> AddAsync(Post post, CancellationToken cancellationToken)
    {
        const string sql = """
        INSERT INTO posts(user_id, post_title, post_body)
        VALUES (:user_id, :post_title, :post_body)
        RETURNING post_id;
        """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("user_id", post.UserId)
            .AddParameter("post_title", post.Title)
            .AddParameter("post_body", post.Body);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);

        long postId = reader.GetInt64("post_id");

        return post with { Id = postId };
    }
}
