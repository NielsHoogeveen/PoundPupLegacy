namespace PoundPupLegacy.EditModel;

public interface Node
{
    int NodeId { get; }
    int UrlId { get; }
    string Title { get; }
    List<Tag> Tags { get;  }
    List<Tenant> Tenants { get; }
    List<TenantNode> TenantNodes { get; }
}
