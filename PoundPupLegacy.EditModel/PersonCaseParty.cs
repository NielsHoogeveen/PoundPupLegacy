namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonCaseParty))]
public partial class PersonCasePartyJsonContext : JsonSerializerContext { }

public record PersonCaseParty
{
    public required PersonListItem Person { get; init; }

    public bool HasBeenDeleted { get; set; } = false;
}
