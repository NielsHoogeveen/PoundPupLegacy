namespace PoundPupLegacy.EditModel;

public record Document: Node
{
    public int? NodeId { get; init; }
    public int? UrlId { get; set; }
    public required string Title { get; set; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public string? SourceUrl { get; set; }
    public required string Text { get; set; }

    public int? DocumentTypeId { get; set; }

    public DateTime? PublicationDateFrom { get; set; }

    public DateTime? PublicationDateTo { get; set; }
    public DateTime? PublicationDate { get; set; }

    public List<DocumentableDocument> documentableDocuments = new();
    public required List<DocumentableDocument> DocumentableDocuments {
        get => documentableDocuments;
        init {
            if (value is not null) {
                documentableDocuments = value;
            }
        }
    }
    public required DocumentType[] DocumentTypes { get; init; }
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

    public List<File> Files {
        get => files;
        init {
            if (value is not null) {
                files = value;
            }
        }
    }

}
