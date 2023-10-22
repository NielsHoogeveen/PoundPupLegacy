namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Subgroup))]
public partial class SubgroupJsonContext : JsonSerializerContext { }

public sealed record Subgroup
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    public required int PublicationStatusIdDefault { get; init; }

    public required bool PublicationStatusDefaultIsOnlyOption { get; init; }
}
