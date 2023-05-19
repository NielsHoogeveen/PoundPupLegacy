namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Persons))]
public partial class PersonsJsonContext : JsonSerializerContext { }

public sealed record Persons : PagedListBase<PersonListEntry>
{
}
