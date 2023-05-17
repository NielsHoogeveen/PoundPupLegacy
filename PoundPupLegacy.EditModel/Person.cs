
using static PoundPupLegacy.EditModel.InterPersonalRelation.InterPersonalRelationFrom.CompletedInterPersonalRelationFrom.ResolvedInterPersonalRelationFrom;

namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPerson))]
public partial class ExistingPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewPerson))]
public partial class NewPersonJsonContext : JsonSerializerContext { }

public interface Person : Party
{
    IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations { get; }

    List<PersonPoliticalEntityRelationTypeListItem> PersonPoliticalEntityRelationTypes { get; }

    IEnumerable<CompletedInterPersonalRelationFrom> InterPersonalRelationsFrom { get; }
    IEnumerable<CompletedInterPersonalRelationTo> InterPersonalRelationsTo { get; }
    List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; }

    IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations { get; }

    List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; }

}
public sealed record NewPerson : PersonBase, NewNode
{
    public override IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations => NewPersonPoliticalEntityRelations;

    public override IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations => NewPersonOrganizationRelations;

    public override IEnumerable<CompletedInterPersonalRelationFrom> InterPersonalRelationsFrom => NewInterPersonalRelationsFrom;
    public override IEnumerable<CompletedInterPersonalRelationTo> InterPersonalRelationsTo => NewInterPersonalRelationsTo;

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
    private List<ExistingInterPersonalRelationFrom> existingInterPersonalRelationsFrom = new();

    public List<ExistingInterPersonalRelationFrom> ExistingInterPersonalRelationsFrom {
        get => existingInterPersonalRelationsFrom;
        init {
            if (value is not null) {
                existingInterPersonalRelationsFrom = value;
            }
        }
    }
    private List<ExistingInterPersonalRelationTo> existingInterPersonalRelationsTo = new();

    public List<ExistingInterPersonalRelationTo> ExistingInterPersonalRelationsTo {
        get => existingInterPersonalRelationsTo;
        init {
            if (value is not null) {
                existingInterPersonalRelationsTo = value;
            }
        }
    }


    public override IEnumerable<CompletedInterPersonalRelationFrom> InterPersonalRelationsFrom => GetInterPersonalRelationsFrom();
    private IEnumerable<CompletedInterPersonalRelationFrom> GetInterPersonalRelationsFrom()
    {
        foreach (var elem in ExistingInterPersonalRelationsFrom) {
            yield return elem;
        }
        foreach (var elem in NewInterPersonalRelationsFrom) {
            yield return elem;
        }

    }
    public override IEnumerable<CompletedInterPersonalRelationTo> InterPersonalRelationsTo => GetInterPersonalRelationsTo();
    private IEnumerable<CompletedInterPersonalRelationTo> GetInterPersonalRelationsTo()
    {
        foreach (var elem in ExistingInterPersonalRelationsTo) {
            yield return elem;
        }
        foreach (var elem in NewInterPersonalRelationsTo) {
            yield return elem;
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

    public abstract IEnumerable<CompletedInterPersonalRelationFrom> InterPersonalRelationsFrom { get; }
    public abstract IEnumerable<CompletedInterPersonalRelationTo> InterPersonalRelationsTo { get; }
    public List<CompletedInterPersonalRelationFrom> NewInterPersonalRelationsFrom { get; set; }  = new();

    public List<CompletedInterPersonalRelationTo> NewInterPersonalRelationsTo { get; set; } = new();

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
