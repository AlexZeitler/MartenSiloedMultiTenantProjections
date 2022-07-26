using Npgsql;

namespace MartenSiloedMultiTenantProjections.Tests.Automation;

public class PostgresAdministration
{
  private readonly string _connectionString;

  public PostgresAdministration(string connectionString)
  {
    _connectionString = connectionString;
  }

  public async Task DropDatabase(string databaseName)
  {
    await using var connection = new NpgsqlConnection { ConnectionString = _connectionString };
    await connection.OpenAsync();
    var command = new NpgsqlCommand($"DROP DATABASE IF EXISTS {databaseName} WITH (FORCE);", connection);
    await command.ExecuteNonQueryAsync();
    await connection.CloseAsync();
  }
}
