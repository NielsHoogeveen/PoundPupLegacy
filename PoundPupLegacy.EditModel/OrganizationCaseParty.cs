namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationCaseParty))]
public partial class OrganizationCasePartyJsonContext : JsonSerializerContext { }

public record OrganizationCaseParty: IComparable<OrganizationCaseParty>
{
    public required OrganizationListItem Organization { get; init; }

    public required bool HasBeenStored { get; init; }

    public bool HasBeenDeleted { get; set; } = false;

    public int CompareTo(OrganizationCaseParty? other)
    {
        if (other is null)
            return -1;
        return Organization.CompareTo(other.Organization);
    }
}
