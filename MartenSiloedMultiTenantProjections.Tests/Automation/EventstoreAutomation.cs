using Marten;
using MartenSiloedMultiTenantProjections.Users;
using Npgsql;
using Weasel.Core;
using static WaitForPostgres.Database;

namespace MartenSiloedMultiTenantProjections.Tests.Automation;

public static class EventstoreAutomation
{
  public static string GetRandomTenantName()
  {
    return $"tenant{Guid.NewGuid().ToString().ToLower().Substring(0, 8)}";
  }

  public static string GetPostgresDbTestConnectionString()
  {
    return GetTestDbConnectionString("postgres");
  }

  public static string GetTestDbConnectionString(string dbName)
  {
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
    {
      Pooling = false,
      Port = 5433,
      Host = "localhost",
      CommandTimeout = 20,
      Database = dbName,
      Password = "123456",
      Username = "postgres"
    };
    var pgTestConnectionString = connectionStringBuilder.ToString();

    return pgTestConnectionString;
  }

  public static async Task<IDocumentStore> GetTestDocumentStore()
  {
    var pgConnectionString = GetPostgresDbTestConnectionString();
    await WaitForConnection(pgConnectionString);

    var store = DocumentStore.For(
      options =>
      {
        options.MultiTenantedWithSingleServer(pgConnectionString);
        options.AutoCreateSchemaObjects = AutoCreate.All;
        options.UseActiveUserProjectionsProjections();
      }
    );

    return store;
  }
}
