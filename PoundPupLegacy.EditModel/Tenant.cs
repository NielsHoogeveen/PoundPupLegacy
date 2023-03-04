namespace PoundPupLegacy.EditModel;

public record Tenant
{
    private Subgroup[] subgroups = Array.Empty<Subgroup>();
    public required Subgroup[] Subgroups {
        get => subgroups;
        init {
            if (value is not null) {
                subgroups = value;
            }
        }
    }

    public required int Id { get; init; }
    public required string DomainName { get; init; }

    public required bool AllowAccess { get; init; }

    public TenantNode? TenantNode { get; set; }

    public bool HasTenantNode => TenantNode != null;
}
