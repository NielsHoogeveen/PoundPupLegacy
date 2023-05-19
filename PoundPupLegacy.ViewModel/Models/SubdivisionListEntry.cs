namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SubdivisionListEntry))]
public partial class SubdivisionListEntryJsonContext : JsonSerializerContext { }

public sealed record SubdivisionListEntry : ListEntryBase
{
}
