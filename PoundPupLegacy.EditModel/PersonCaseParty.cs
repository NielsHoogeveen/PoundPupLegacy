namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonCaseParty))]
public partial class PersonCasePartyJsonContext : JsonSerializerContext { }

public record PersonCaseParty: IComparable<PersonCaseParty>
{
    public required PersonListItem Person { get; init; }

    public required bool HasBeenStored { get; init; }

    public bool HasBeenDeleted { get; set; } = false;

    public int CompareTo(PersonCaseParty? other)
    {
        if (other is null)
            return -1;
        return Person.CompareTo(other.Person);
    }

}
