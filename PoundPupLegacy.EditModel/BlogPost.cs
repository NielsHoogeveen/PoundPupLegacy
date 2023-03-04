namespace PoundPupLegacy.EditModel;

public record BlogPost : SimpleTextNode
{
    public required int NodeId { get; init; }

    public required int UrlId { get; init; }

    public required string Title { get; set; }

    public required string Text { get; set; }

    private List<Tag> tags = new();

    public List<Tag> Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }
        }
    }
    private List<TenantNode> tenantNodes = new();

    public List<TenantNode> TenantNodes {
        get => tenantNodes;
        init {
            if (value is not null) {
                tenantNodes = value;
            }
        }
    }
    private List<Tenant> tenants = new();

    public List<Tenant> Tenants {
        get => tenants;
        init {
            if (value is not null) {
                tenants = value;
            }
        }
    }
}
