namespace PoundPupLegacy.EditModel;

public record PartyBase: LocatableBase, Party
{
    private List<PartyPoliticalEntityRelation> partyPoliticalEntityRelations = new();

    public List<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations {
        get => partyPoliticalEntityRelations;
        init {
            if (value is not null) {
                partyPoliticalEntityRelations = value;
            }
        }
    }

    private List<PartyPoliticalEntityRelationTypeListItem> partyPoliticalEntityRelationTypes = new();

    public List<PartyPoliticalEntityRelationTypeListItem> PartyPoliticalEntityRelationTypes {
        get => partyPoliticalEntityRelationTypes;
        init {
            if (value is not null) {
                partyPoliticalEntityRelationTypes = value;
            }
        }
    }
    private List<PersonOrganizationRelation> personOrganizationRelations = new();

    public List<PersonOrganizationRelation> PersonOrganizationRelations {
        get => personOrganizationRelations;
        init {
            if (value is not null) {
                personOrganizationRelations = value;
            }
        }
    }

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
    List<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations { get; }

    List<PartyPoliticalEntityRelationTypeListItem> PartyPoliticalEntityRelationTypes { get; }

    List<PersonOrganizationRelation> PersonOrganizationRelations { get; }

    List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; }
}
