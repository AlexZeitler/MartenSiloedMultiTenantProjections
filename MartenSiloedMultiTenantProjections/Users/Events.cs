namespace MartenSiloedMultiTenantProjections.Users;

public class Subscribed
{
  public Guid Id { get; set; }
  public string Username { get; set; }
  public DateTimeOffset SubscribedOn { get; set; }
}

public class Disabled
{
  public Guid Id { get; set; }
  public DateTimeOffset DisabledOn { get; set; }
}

public class Enabled
{
  public Guid Id { get; set; }
  public DateTimeOffset EnabledOn { get; set; }
}

public class Closed
{
  public Guid Id { get; set; }
  public DateTimeOffset ClosedOn { get; set; }
}

