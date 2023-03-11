namespace PoundPupLegacy.EditModel;

public record Discussion : SimpleTextNode
{
    public int? NodeId { get; init; }

    public int? UrlId { get; set; }

    public required string Title { get; set; }

    public required string Text { get; set; }
    public required int PublisherId { get; set; }

    public required int OwnerId { get; set; }

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
    private List<File> files = new();

    public required List<File> Files {
        get => files;
        init {
            if (value is not null) {
                files = value;
            }
        }
    }

}
