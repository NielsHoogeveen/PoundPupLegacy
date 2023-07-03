namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TenantDetails))]
public partial class TenantDetailsJsonContext : JsonSerializerContext { }

public sealed record TenantDetails
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

    public required int PublicationStatusIdDefault { get; init; }

    public required bool AllowAccess { get; init; }

    public TenantNode? TenantNode { get; set; }

    public bool HasTenantNode => TenantNode != null;
}
