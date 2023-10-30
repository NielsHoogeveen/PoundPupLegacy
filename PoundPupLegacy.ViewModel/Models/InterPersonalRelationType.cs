namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(InterPersonalRelationType))]
public partial class InterPersonalRelationTypeJsonContext : JsonSerializerContext { }

public sealed record InterPersonalRelationType: NameableBase
{
}
