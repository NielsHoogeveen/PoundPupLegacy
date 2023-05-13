namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Organizations))]
public partial class OrganizationsJsonContext : JsonSerializerContext { }

public record Organizations : IPagedList<OrganizationListEntry>
{
    private OrganizationListEntry[] _entries = Array.Empty<OrganizationListEntry>();
    public required OrganizationListEntry[] Entries {
        get => _entries;
        set {
            if (value is not null) {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }
}
