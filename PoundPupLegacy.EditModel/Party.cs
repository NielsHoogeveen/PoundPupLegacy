namespace PoundPupLegacy.EditModel;

public abstract record PartyBase: LocatableBase, Party
{
    public List<CompletedPartyPoliticalEntityRelation> NewPartyPoliticalEntityRelations { get; } = new();

    private List<PartyPoliticalEntityRelationTypeListItem> partyPoliticalEntityRelationTypes = new();

    public List<PartyPoliticalEntityRelationTypeListItem> PartyPoliticalEntityRelationTypes {
        get => partyPoliticalEntityRelationTypes;
        init {
            if (value is not null) {
                partyPoliticalEntityRelationTypes = value;
            }
        }
    }
    public abstract IEnumerable<CompletedPartyPoliticalEntityRelation> PartyPoliticalEntityRelations { get; }
    public abstract IEnumerable<CompletedPersonOrganizationRelation> PersonOrganizationRelations { get; }
    public List<CompletedNewPersonOrganizationRelation> NewPersonOrganizationRelations { get; } = new();

    private List<PersonOrganizationRelationTypeListItem> personOrganizationRelationTypes = new();
    public List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes {
        get => personOrganizationRelationTypes;
        init {
            if (value is not null) {
                personOrganizationRelationTypes = value;
            }
        }
    }
}

public interface Party : Locatable
{
    IEnumerable<CompletedPartyPoliticalEntityRelation> PartyPoliticalEntityRelations { get; }

    List<PartyPoliticalEntityRelationTypeListItem> PartyPoliticalEntityRelationTypes { get; }

    IEnumerable<CompletedPersonOrganizationRelation> PersonOrganizationRelations { get; }

    List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; }
}
