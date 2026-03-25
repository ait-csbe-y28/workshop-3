using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace ObservabilitySample.Service.Infrastructure.Persistence.Migrations;

[Migration(1774468796)]
public sealed class Initial :SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
    """
    CREATE TABLE users
    (
        user_id bigint PRIMARY KEY GENERATED ALWAYS AS IDENTITY ,
        user_login text NOT NULL ,
        user_name text NOT NULL 
    );

    CREATE UNIQUE INDEX users_user_login_idx ON users(user_login);

    CREATE TABLE posts
    (
      post_id bigint PRIMARY KEY GENERATED ALWAYS AS IDENTITY ,
      user_id bigint NOT NULL REFERENCES users(user_id),
      post_title text NOT NULL ,
      post_body text NOT NULL 
    );
    """;
    
    protected override string GetDownSql(IServiceProvider serviceProvider) =>
    """
    DROP TABLE posts;
    DROP TABLE users;
    """;
}
