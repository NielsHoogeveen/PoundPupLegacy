using static PoundPupLegacy.EditModel.InterOrganizationalRelation.InterOrganizationalRelationFrom;
using static PoundPupLegacy.EditModel.InterOrganizationalRelation.InterOrganizationalRelationTo;

namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingOrganization))]
public partial class ExistingOrganizationJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewOrganization))]
public partial class NewOrganizationJsonContext : JsonSerializerContext { }

public interface Organization : Party, ResolvedNode
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


public sealed record ExistingOrganization : ExistingPartyBase, Organization
{
    private List<ExistingOrganizationPoliticalEntityRelation> existingOrganizationPoliticalEntityRelations = new();

    public List<ExistingOrganizationPoliticalEntityRelation> ExistingOrganizationPoliticalEntityRelations {
        get => existingOrganizationPoliticalEntityRelations;
        init {
            if (value is not null) {
                existingOrganizationPoliticalEntityRelations = value;
            }
        }
    }
    public IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations => GetOrganizationPoliticalEntityRelations();
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
    public IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom => GetInterOrganizationalRelationsFrom();
    private IEnumerable<CompletedInterOrganizationalRelationFrom> GetInterOrganizationalRelationsFrom()
    {
        foreach (var elem in ExistingInterOrganizationalRelationsFrom) {
            yield return elem;
        }
        foreach (var elem in NewInterOrganizationalRelationsFrom) {
            yield return elem;
        }
    }
    public IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo => GetInterOrganizationalRelationsTo();
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


    public IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations => GetPersonOrganizationRelations();
    private IEnumerable<CompletedPersonOrganizationRelationForOrganization> GetPersonOrganizationRelations()
    {
        foreach (var elem in ExistingPersonOrganizationRelations) {
            yield return elem;
        }
        foreach (var elem in NewPersonOrganizationRelations) {
            yield return elem;
        }
    }

    public OrganizationItem OrganizationItem => new OrganizationListItem { Id = NodeId, Name = Title };

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
public sealed record NewOrganization : NewPartyBase, ResolvedNewNode, Organization
{
    public IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations => NewOrganizationPoliticalEntityRelations;
    public IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom => NewInterOrganizationalRelationsFrom;
    public IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo => NewInterOrganizationalRelationsTo;
    public IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations => NewPersonOrganizationRelations;
    public OrganizationItem OrganizationItem => new OrganizationName { Name = Title };

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
