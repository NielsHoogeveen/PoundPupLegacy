namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPerson))]
public partial class ExistingPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewPerson))]
public partial class NewPersonJsonContext : JsonSerializerContext { }

public interface Person : Party
{
    List<InterPersonalRelation> InterPersonalRelations { get; }
    List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; }
}
public record NewPerson : PersonBase, NewNode 
{
        public override IEnumerable<CompletedPartyPoliticalEntityRelation> PartyPoliticalEntityRelations => NewPartyPoliticalEntityRelations;

    public override IEnumerable<CompletedPersonOrganizationRelation> PersonOrganizationRelations => NewPersonOrganizationRelations;

}
public record ExistingPerson : PersonBase, ExistingNode
{
    public required int NodeId { get; init; }
    public required int UrlId { get; set; }

    private List<ExistingPartyPoliticalEntityRelation> existingPartyPoliticalEntityRelations = new();

    public List<ExistingPartyPoliticalEntityRelation> ExistingPartyPoliticalEntityRelations {
        get => existingPartyPoliticalEntityRelations;
        init {
            if (value is not null) {
                existingPartyPoliticalEntityRelations = value;
            }
        }
    }
    public override IEnumerable<CompletedPartyPoliticalEntityRelation> PartyPoliticalEntityRelations => GetPartyPoliticalEntityRelations();
    private IEnumerable<CompletedPartyPoliticalEntityRelation> GetPartyPoliticalEntityRelations()
    {
        foreach (var elem in ExistingPartyPoliticalEntityRelations) {
            yield return elem;
        }
        foreach (var elem in NewPartyPoliticalEntityRelations) {
            yield return elem;
        }
    }



    private List<ExistingPersonOrganizationRelation> existingPersonOrganizationRelations = new();

    public List<ExistingPersonOrganizationRelation> ExistingPersonOrganizationRelations {
        get => existingPersonOrganizationRelations;
        init {
            if (value is not null) {
                existingPersonOrganizationRelations = value;
            }
        }
    }


    public override IEnumerable<CompletedPersonOrganizationRelation> PersonOrganizationRelations => GetPersonOrganizationRelations();
    private IEnumerable<CompletedPersonOrganizationRelation> GetPersonOrganizationRelations()
    {
        foreach (var elem in ExistingPersonOrganizationRelations) {
            yield return elem;
        }
        foreach (var elem in NewPersonOrganizationRelations) {
            yield return elem;
        }

    }

}
public abstract record PersonBase : PartyBase, Person
{
    private List<InterPersonalRelation> interPersonalRelations = new();

    public List<InterPersonalRelation> InterPersonalRelations {
        get => interPersonalRelations;
        init {
            if (value is not null) {
                interPersonalRelations = value;
            }
        }
    }

    private List<InterPersonalRelationTypeListItem> interPersonalRelationTypes = new();

    public List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes {
        get => interPersonalRelationTypes;
        init {
            if (value is not null) {
                interPersonalRelationTypes = value;
            }
        }
    }
}
