using Marten;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

namespace MartenSiloedMultiTenantProjections.Users;

public class ActiveUser
{
  
  public DateTimeOffset SubscribedOn { get; set; }
  public string Username { get; set; }
  public Guid Id { get; set; }

  internal bool ShouldDelete(Disabled disabled) => true;
  
  public static ActiveUser Create(
    Subscribed subscribed
  )
  {
    var tenant = new ActiveUser { Username = subscribed.Username, SubscribedOn = subscribed.SubscribedOn };
    return tenant;
  }
  
  

}

public static class ActiveUserProjectionConfiguration
{
  public static void UseActiveUserProjectionsProjections(
    this StoreOptions options
  )
  {
    options.Projections.SelfAggregate<ActiveUser>(ProjectionLifecycle.Inline);
  }
}
