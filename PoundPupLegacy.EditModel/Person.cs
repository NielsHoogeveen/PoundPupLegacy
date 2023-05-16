using static PoundPupLegacy.EditModel.PersonOrganizationRelation.PersonOrganizationRelationForPerson;
using static PoundPupLegacy.EditModel.PersonOrganizationRelation.PersonOrganizationRelationForPerson.CompletedPersonOrganizationRelationForPerson.ResolvedPersonOrganizationRelationForPerson;
using static PoundPupLegacy.EditModel.PersonPoliticalEntityRelation;
using static PoundPupLegacy.EditModel.PersonPoliticalEntityRelation.CompletedPersonPoliticalEntityRelation.ResolvedPersonPoliticalEntityRelation;
using static PoundPupLegacy.EditModel.InterPersonalRelation;
namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPerson))]
public partial class ExistingPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewPerson))]
public partial class NewPersonJsonContext : JsonSerializerContext { }

public interface Person : Party
{
    IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations { get; }

    List<PersonPoliticalEntityRelationTypeListItem> PersonPoliticalEntityRelationTypes { get; }

    List<CompletedInterPersonalRelation> InterPersonalRelations { get; }
    List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; }

    IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations { get; }

    List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; }

}
public sealed record NewPerson : PersonBase, NewNode 
{
    public override IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations => NewPersonPoliticalEntityRelations;

    public override IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations => NewPersonOrganizationRelations;

}
public sealed record ExistingPerson : PersonBase, ExistingNode
{
    public required int NodeId { get; init; }
    public required int UrlId { get; set; }

    private List<ExistingPersonPoliticalEntityRelation> existingPersonPoliticalEntityRelations = new();

    public List<ExistingPersonPoliticalEntityRelation> ExistingPersonPoliticalEntityRelations {
        get => existingPersonPoliticalEntityRelations;
        init {
            if (value is not null) {
                existingPersonPoliticalEntityRelations = value;
            }
        }
    }
    public override IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations => GetPersonPoliticalEntityRelations();
    private IEnumerable<CompletedPersonPoliticalEntityRelation> GetPersonPoliticalEntityRelations()
    {
        foreach (var elem in ExistingPersonPoliticalEntityRelations) {
            yield return elem;
        }
        foreach (var elem in NewPersonPoliticalEntityRelations) {
            yield return elem;
        }
    }



    private List<ExistingPersonOrganizationRelationForPerson> existingPersonOrganizationRelations = new();

    public List<ExistingPersonOrganizationRelationForPerson> ExistingPersonOrganizationRelations {
        get => existingPersonOrganizationRelations;
        init {
            if (value is not null) {
                existingPersonOrganizationRelations = value;
            }
        }
    }


    public override IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations => GetPersonOrganizationRelations();
    private IEnumerable<CompletedPersonOrganizationRelationForPerson> GetPersonOrganizationRelations()
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
    public List<CompletedPersonPoliticalEntityRelation> NewPersonPoliticalEntityRelations { get; } = new();

    private List<PersonPoliticalEntityRelationTypeListItem> personPoliticalEntityRelationTypes = new();

    public List<PersonPoliticalEntityRelationTypeListItem> PersonPoliticalEntityRelationTypes {
        get => personPoliticalEntityRelationTypes;
        init {
            if (value is not null) {
                personPoliticalEntityRelationTypes = value;
            }
        }
    }
    public abstract IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations { get; }

    private List<CompletedInterPersonalRelation> interPersonalRelations = new();

    public List<CompletedInterPersonalRelation> InterPersonalRelations {
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
    public abstract IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations { get; }
    public List<CompletedNewPersonOrganizationRelationForPerson> NewPersonOrganizationRelations { get; } = new();

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
