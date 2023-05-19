using static PoundPupLegacy.EditModel.InterOrganizationalRelation.InterOrganizationalRelationFrom;
using static PoundPupLegacy.EditModel.InterOrganizationalRelation.InterOrganizationalRelationTo;

namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingOrganization))]
public partial class ExistingOrganizationJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewOrganization))]
public partial class NewOrganizationJsonContext : JsonSerializerContext { }

public interface Organization : Party
{
    IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations { get; }

    List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; }

    IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations { get; }

    List<OrganizationPoliticalEntityRelationTypeListItem> OrganizationPoliticalEntityRelationTypes { get; }


    string? WebSiteUrl { get; set; }
    string? EmailAddress { get; set; }
    FuzzyDate? Establishment { get; set; }
    FuzzyDate? Termination { get; set; }
    List<OrganizationOrganizationType> OrganizationOrganizationTypes { get; }
    List<OrganizationType> OrganizationTypes { get; }
    List<InterOrganizationalRelationTypeListItem> InterOrganizationalRelationTypes { get; }
    IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom { get; }
    IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo { get; }
    OrganizationItem OrganizationItem { get; }
}


public sealed record ExistingOrganization : OrganizationBase, ExistingNode
{
    public int NodeId { get; init; }
    public int UrlId { get; set; }

    private List<ExistingOrganizationPoliticalEntityRelation> existingOrganizationPoliticalEntityRelations = new();

    public List<ExistingOrganizationPoliticalEntityRelation> ExistingOrganizationPoliticalEntityRelations {
        get => existingOrganizationPoliticalEntityRelations;
        init {
            if (value is not null) {
                existingOrganizationPoliticalEntityRelations = value;
            }
        }
    }
    public override IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations => GetOrganizationPoliticalEntityRelations();
    private IEnumerable<CompletedOrganizationPoliticalEntityRelation> GetOrganizationPoliticalEntityRelations()
    {
        foreach (var elem in ExistingOrganizationPoliticalEntityRelations) {
            yield return elem;
        }
        foreach (var elem in NewOrganizationPoliticalEntityRelations) {
            yield return elem;
        }
    }

    private List<ExistingInterOrganizationalRelationFrom> existingInterOrganizationalRelationsFrom = new();

    public List<ExistingInterOrganizationalRelationFrom> ExistingInterOrganizationalRelationsFrom {
        get => existingInterOrganizationalRelationsFrom;
        init {
            if (value is not null) {
                existingInterOrganizationalRelationsFrom = value;
            }
        }
    }
    private List<ExistingInterOrganizationalRelationTo> existingInterOrganizationalRelationsTo = new();

    public List<ExistingInterOrganizationalRelationTo> ExistingInterOrganizationalRelationsTo {
        get => existingInterOrganizationalRelationsTo;
        init {
            if (value is not null) {
                existingInterOrganizationalRelationsTo = value;
            }
        }
    }
    public override IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom => GetInterOrganizationalRelationsFrom();
    private IEnumerable<CompletedInterOrganizationalRelationFrom> GetInterOrganizationalRelationsFrom()
    {
        foreach (var elem in ExistingInterOrganizationalRelationsFrom) {
            yield return elem;
        }
        foreach (var elem in NewInterOrganizationalRelationsFrom) {
            yield return elem;
        }
    }
    public override IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo => GetInterOrganizationalRelationsTo();
    private IEnumerable<CompletedInterOrganizationalRelationTo> GetInterOrganizationalRelationsTo()
    {
        foreach (var elem in ExistingInterOrganizationalRelationsTo) {
            yield return elem;
        }
        foreach (var elem in NewInterOrganizationalRelationsTo) {
            yield return elem;
        }
    }
    private List<ExistingPersonOrganizationRelationForOrganization> existingPersonOrganizationRelations = new();

    public List<ExistingPersonOrganizationRelationForOrganization> ExistingPersonOrganizationRelations {
        get => existingPersonOrganizationRelations;
        init {
            if (value is not null) {
                existingPersonOrganizationRelations = value;
            }
        }
    }


    public override IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations => GetPersonOrganizationRelations();
    private IEnumerable<CompletedPersonOrganizationRelationForOrganization> GetPersonOrganizationRelations()
    {
        foreach (var elem in ExistingPersonOrganizationRelations) {
            yield return elem;
        }
        foreach (var elem in NewPersonOrganizationRelations) {
            yield return elem;
        }

    }

    public override OrganizationItem OrganizationItem => new OrganizationListItem { Id = NodeId, Name = Title };
}
public sealed record NewOrganization : OrganizationBase, NewNode
{
    public override IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations => NewOrganizationPoliticalEntityRelations;
    public override IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom => NewInterOrganizationalRelationsFrom;
    public override IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo => NewInterOrganizationalRelationsTo;
    public override IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations => NewPersonOrganizationRelations;
    public override OrganizationItem OrganizationItem => new OrganizationName { Name = Title };
}
public abstract record OrganizationBase : PartyBase, Organization
{

    public List<CompletedOrganizationPoliticalEntityRelation> NewOrganizationPoliticalEntityRelations { get; } = new();

    private List<OrganizationPoliticalEntityRelationTypeListItem> personPoliticalEntityRelationTypes = new();

    public List<OrganizationPoliticalEntityRelationTypeListItem> OrganizationPoliticalEntityRelationTypes {
        get => personPoliticalEntityRelationTypes;
        init {
            if (value is not null) {
                personPoliticalEntityRelationTypes = value;
            }
        }
    }
    public abstract IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations { get; }

    public string? WebSiteUrl { get; set; }
    public string? EmailAddress { get; set; }
    public FuzzyDate? Establishment { get; set; }
    public FuzzyDate? Termination { get; set; }

    private List<OrganizationOrganizationType> organizationOrganizationTypes = new();

    public List<OrganizationOrganizationType> OrganizationOrganizationTypes {
        get => organizationOrganizationTypes;
        init {
            if (value is not null) {
                organizationOrganizationTypes = value;
            }
        }
    }
    private List<OrganizationType> organizationTypes = new();

    public List<OrganizationType> OrganizationTypes {
        get => organizationTypes;
        init {
            if (value is not null) {
                organizationTypes = value;
            }
        }
    }

    public List<CompletedInterOrganizationalRelationFrom> NewInterOrganizationalRelationsFrom { get; } = new();
    public List<CompletedInterOrganizationalRelationTo> NewInterOrganizationalRelationsTo { get; } = new();

    private List<InterOrganizationalRelationTypeListItem> interOrganizationalRelationTypes = new();

    public List<InterOrganizationalRelationTypeListItem> InterOrganizationalRelationTypes {
        get => interOrganizationalRelationTypes;
        init {
            if (value is not null) {
                interOrganizationalRelationTypes = value;
            }
        }
    }
    public abstract OrganizationItem OrganizationItem { get; }
    public abstract IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom { get; }
    public abstract IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo { get; }

    public abstract IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations { get; }
    public List<CompletedNewPersonOrganizationRelationForOrganization> NewPersonOrganizationRelations { get; } = new();

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
