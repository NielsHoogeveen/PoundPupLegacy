namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationType))]
public partial class OrganizationTypeJsonContext : JsonSerializerContext { }

public record OrganizationType
{
    public int Id { get; init; }

    public required string Name { get; init; }
}
