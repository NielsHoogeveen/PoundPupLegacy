namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Person))]
public partial class PersonJsonContext : JsonSerializerContext { }

public sealed record Person : NameableBase
{

    public DateTime? DateOfBirth { get; init; }
    public DateTime? DateOfDeath { get; init; }
    public Image? Portrait { get; init; }
    public string? FirstName { get; init; }
    public string? MiddleName { get; init; }
    public string? LastName { get; init; }
    public string? FullName { get; init; }
    public string? Suffix { get; init; }
    public string? NickName { get; init; }
    private BasicLink[] professions = Array.Empty<BasicLink>();
    public required BasicLink[] Professions {
        get => professions;
        init {
            if (value is not null) {
                professions = value;
            }
        }
    }

    private InterPersonalRelation[] interPersonalRelations = Array.Empty<InterPersonalRelation>();
    public required InterPersonalRelation[] InterPersonalRelations {
        get => interPersonalRelations;
        init {
            if (value is not null) {
                interPersonalRelations = value;
            }
        }
    }
    private PartyCaseType[] partyCaseTypes = Array.Empty<PartyCaseType>();
    public required PartyCaseType[] PartyCaseTypes {
        get => partyCaseTypes;
        init {
            if (value is not null) {
                partyCaseTypes = value;
            }
        }
    }
    private OrganizationPersonRelation[] organizationPersonRelations = Array.Empty<OrganizationPersonRelation>();
    public required OrganizationPersonRelation[] OrganizationPersonRelations {
        get => organizationPersonRelations;
        init {
            if (value is not null) {
                organizationPersonRelations = value;
            }
        }
    }
    private PartyPoliticalEntityRelation[] partyPoliticalEntityRelations = Array.Empty<PartyPoliticalEntityRelation>();
    public required PartyPoliticalEntityRelation[] PartyPoliticalEntityRelations {
        get => partyPoliticalEntityRelations;
        init {
            if (value is not null) {
                partyPoliticalEntityRelations = value;
            }
        }
    }
    private BillAction[] _billActions = Array.Empty<BillAction>();
    public required BillAction[] BillActions {
        get => _billActions;
        init {
            if (value is not null) {
                _billActions = value;
            }
        }
    }
}
