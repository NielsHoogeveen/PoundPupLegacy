namespace PoundPupLegacy.EditModel;

public abstract record PartyBase: LocatableBase, Party
{
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

    IEnumerable<CompletedPersonOrganizationRelation> PersonOrganizationRelations { get; }

    List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; }
}
