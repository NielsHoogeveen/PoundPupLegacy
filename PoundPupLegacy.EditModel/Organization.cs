namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingOrganization))]
public partial class ExistingOrganizationJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewOrganization))]
public partial class NewOrganizationJsonContext : JsonSerializerContext { }

public interface Organization : Party
{
    IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations { get; }

    List<OrganizationPoliticalEntityRelationTypeListItem> OrganizationPoliticalEntityRelationTypes { get; }


    string? WebSiteUrl { get; set; }
    string? EmailAddress { get; set; }
    DateTime? EstablishmentDateFrom { get; set; }
    DateTime? EstablishmentDateTo { get; set; }
    DateTime? TerminationDateFrom { get; set; }
    DateTime? TerminationDateTo { get; set; }
    FuzzyDate? Establishment { get; set; }
    FuzzyDate? Termination { get; set; }
    List<OrganizationOrganizationType> OrganizationOrganizationTypes { get; }
    List<OrganizationType> OrganizationTypes { get; }
    List<InterOrganizationalRelationTypeListItem> InterOrganizationalRelationTypes { get; }
    IEnumerable<CompletedInterOrganizationalRelation> InterOrganizationalRelations { get; }
    PartyItem.OrganizationItem OrganizationItem { get; }
}


public record ExistingOrganization : OrganizationBase, ExistingNode
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

    private List<ExistingInterOrganizationalRelation> existingInterOrganizationalRelations = new();

    public List<ExistingInterOrganizationalRelation> ExistingInterOrganizationalRelations {
        get => existingInterOrganizationalRelations;
        init {
            if (value is not null) {
                existingInterOrganizationalRelations = value;
            }
        }
    }
    public override IEnumerable<CompletedInterOrganizationalRelation> InterOrganizationalRelations => GetInterOrganizationalRelations();
    private IEnumerable<CompletedInterOrganizationalRelation> GetInterOrganizationalRelations()
    {
        foreach (var elem in ExistingInterOrganizationalRelations) {
            yield return elem;
        }
        foreach (var elem in NewInterOrganizationalRelations) {
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

    public override PartyItem.OrganizationItem OrganizationItem => new PartyItem.OrganizationItem.OrganizationListItem { Id = NodeId, Name = Title };
}
public record NewOrganization : OrganizationBase, NewNode
{
    public override IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations => NewOrganizationPoliticalEntityRelations;
    public override IEnumerable<CompletedInterOrganizationalRelation> InterOrganizationalRelations => NewInterOrganizationalRelations;
    public override IEnumerable<CompletedPersonOrganizationRelation> PersonOrganizationRelations => NewPersonOrganizationRelations;
    public override PartyItem.OrganizationItem OrganizationItem => new PartyItem.OrganizationItem.OrganizationName { Name = Title };
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
    public DateTime? EstablishmentDateFrom { get; set; }
    public DateTime? EstablishmentDateTo { get; set; }
    public DateTime? TerminationDateFrom { get; set; }
    public DateTime? TerminationDateTo { get; set; }

    private bool _establishmentSet;
    private FuzzyDate? _establishment;
    public FuzzyDate? Establishment {
        get {
            if (!_establishmentSet) {
                if (EstablishmentDateFrom is not null && EstablishmentDateTo is not null) {
                    var dateTimeRange = new DateTimeRange(EstablishmentDateFrom, EstablishmentDateTo);
                    if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                        _establishment = result;
                    }
                }
                else {
                    _establishment = null;
                }
                _establishmentSet = true;
            }
            return _establishment;
        }
        set {
            _establishment = value;
        }
    }
    private bool _terminationSet;
    private FuzzyDate? _termination;
    public FuzzyDate? Termination {
        get {
            if (!_terminationSet) {
                if (TerminationDateFrom is not null && TerminationDateTo is not null) {
                    var dateTimeRange = new DateTimeRange(TerminationDateFrom, TerminationDateTo);
                    if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                        _termination = result;
                    }
                }
                else {
                    _termination = null;
                }
                _terminationSet = true;
            }
            return _termination;
        }
        set {
            _termination = value;
        }
    }

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

    public List<CompletedNewInterOrganizationalRelation> NewInterOrganizationalRelations { get; } = new();

    private List<InterOrganizationalRelationTypeListItem> interOrganizationalRelationTypes = new();

    public List<InterOrganizationalRelationTypeListItem> InterOrganizationalRelationTypes {
        get => interOrganizationalRelationTypes;
        init {
            if (value is not null) {
                interOrganizationalRelationTypes = value;
            }
        }
    }
    public abstract PartyItem.OrganizationItem OrganizationItem { get; }
    public abstract IEnumerable<CompletedInterOrganizationalRelation> InterOrganizationalRelations { get; }
}
