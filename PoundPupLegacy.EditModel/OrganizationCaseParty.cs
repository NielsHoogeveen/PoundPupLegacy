namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationCaseParty))]
public partial class OrganizationCasePartyJsonContext : JsonSerializerContext { }

public record OrganizationCaseParty
{
    public required OrganizationListItem Organization { get; init; }

    public bool HasBeenDeleted { get; set; } = false;
}
