using Marten;
using MartenSiloedMultiTenantProjections.Tests.Automation;
using MartenSiloedMultiTenantProjections.Users;
using Shouldly;
using Weasel.Core;

namespace MartenSiloedMultiTenantProjections.Tests;

using static EventstoreAutomation;

public class UserProjectionTests
{
  [Fact]
  public async Task ShouldProjectSubscribeForSingleTenant()
  {
    var pgAdmin = new PostgresAdministration(GetPostgresDbTestConnectionString());
    var tenantName = GetRandomTenantName();
    var store = await GetTestDocumentStore();

    await using var session = store.OpenSession(tenantName);

    var username = "jane.doe@tempuri.org";
    var subscribed = new Subscribed { Id = Guid.NewGuid(), Username = username, SubscribedOn = DateTimeOffset.Now };
    var streamId = Guid.NewGuid();

    session.Events.StartStream(streamId, subscribed);
    await session.SaveChangesAsync();

    var user = await session.LoadAsync<ActiveUser>(streamId);
    await pgAdmin.DropDatabase(tenantName);

    user.Username.ShouldBe(username);
  }

  [Fact]
  public async Task ShouldProjectUserDisabledEventForMultipleTenants()
  {
    var pgAdmin = new PostgresAdministration(GetPostgresDbTestConnectionString());
    var tenantName = GetRandomTenantName();
    var store = await GetTestDocumentStore();

    await using var session = store.OpenSession(tenantName);

    var username = "jane.doe@tempuri.org";
    var subscribed = new Subscribed { Id = Guid.NewGuid(), Username = username, SubscribedOn = DateTimeOffset.Now };
    var streamId = Guid.NewGuid();

    session.Events.StartStream(streamId, subscribed);
    await session.SaveChangesAsync();

    var disabled = new Disabled { Id = Guid.NewGuid(), DisabledOn = DateTimeOffset.Now };
    session.Events.Append(streamId, disabled);
    await session.SaveChangesAsync();

    var user = await session.LoadAsync<ActiveUser>(streamId);
    await pgAdmin.DropDatabase(tenantName);

    user.ShouldBeNull();
  }
}
