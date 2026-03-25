using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Npgsql;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Queries;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Repositories;
using ObservabilitySample.Service.Application.Abstractions.Persistence.Results;
using ObservabilitySample.Service.Application.Models;

namespace ObservabilitySample.Service.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public UserRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async IAsyncEnumerable<User> QueryAsync(
        UserQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT  user_id,
                user_login,
                user_name
        FROM users
        WHERE user_id = ANY(:user_ids);
        """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("user_ids", query.UserIds);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new User(
                Id: reader.GetInt64("user_id"),
                Login: reader.GetString("user_login"),
                Name: reader.GetString("user_name"));
        }
    }

    public async Task<AddUserResult> TryAddAsync(User user, CancellationToken cancellationToken)
    {
        const string sql = """
        INSERT INTO users(user_login, user_name)
        VALUES (:user_login, :user_name)
        RETURNING user_id;
        """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("user_login", user.Login)
            .AddParameter("user_name", user.Name);

        try
        {
            await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            await reader.ReadAsync(cancellationToken);

            long userId = reader.GetInt64("user_id");

            return new AddUserResult.Success(user with { Id = userId });
        }
        catch (PostgresException e) when (e.ConstraintName is "users_user_login_idx")
        {
            return new AddUserResult.LoginConflict();
        }
    }
}
