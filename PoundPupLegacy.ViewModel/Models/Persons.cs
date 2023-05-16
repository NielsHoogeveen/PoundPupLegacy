namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Persons))]
public partial class PersonsJsonContext : JsonSerializerContext { }

public sealed record Persons : IPagedList<PersonListEntry>
{
    private PersonListEntry[] _entries = Array.Empty<PersonListEntry>();
    public required PersonListEntry[] Entries {
        get => _entries;
        init {
            if (value is not null) {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }

}
