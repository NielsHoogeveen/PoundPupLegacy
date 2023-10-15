namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationTypeListItem))]
public partial class OrganizationTypeListItemJsonContext : JsonSerializerContext { }

public sealed record OrganizationTypeListItem
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required bool HasConcreteSubtype { get; init; }
}
