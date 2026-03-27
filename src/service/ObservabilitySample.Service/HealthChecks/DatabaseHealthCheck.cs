using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace ObservabilitySample.Service.HealthChecks;

public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly NpgsqlDataSource _dataSource;

    public DatabaseHealthCheck(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

            await using NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT 1";

            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (NpgsqlException)
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}
