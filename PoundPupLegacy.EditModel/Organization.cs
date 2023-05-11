namespace PoundPupLegacy.EditModel;

public record Organization : Party
{
    public int? NodeId { get; init; }
    public int? UrlId { get; set; }
    public required string NodeTypeName { get; set; }
    public required string Title { get; set; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public required string Description { get; set; }
    public string? WebSiteUrl { get; set; }
    public string? EmailAddress { get; set; }
    public DateTime? EstablishmentDateFrom { get; init; }
    public DateTime? EstablishmentDateTo { get; init; }
    public DateTime? TerminationDateFrom { get; init; }
    public DateTime? TerminationDateTo { get; init; }

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

    private List<Tags> tags = new();

    public List<Tags> Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }
        }
    }

    private List<TenantNode> tenantNodes = new();

    public List<TenantNode> TenantNodes {
        get => tenantNodes;
        init {
            if (value is not null) {
                tenantNodes = value;
            }
        }
    }
    private List<Tenant> tenants = new();

    public List<Tenant> Tenants {
        get => tenants;
        init {
            if (value is not null) {
                tenants = value;
            }
        }
    }
    private List<File> files = new();

    public List<File> Files {
        get => files;
        init {
            if (value is not null) {
                files = value;
            }
        }
    }
    private List<Location> locations = new();

    public List<Location> Locations {
        get => locations;
        init {
            if (value is not null) {
                locations = value;
            }
        }
    }
    private List<Term> terms = new();

    public List<Term> Terms {
        get => terms;
        init {
            if (value is not null) {
                terms = value;
            }
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
    private List<CountryListItem> countries = new();

    public List<CountryListItem> Countries {
        get => countries;
        init {
            if (value is not null) {
                countries = value;
            }
        }
    }

    private List<InterOrganizationalRelation> interOrganizationalRelations = new();

    public List<InterOrganizationalRelation> InterOrganizationalRelations {
        get => interOrganizationalRelations;
        init {
            if (value is not null) {
                interOrganizationalRelations = value;
            }
        }
    }

    private List<InterOrganizationalRelationTypeListItem> interOrganizationalRelationTypes = new();

    public List<InterOrganizationalRelationTypeListItem> InterOrganizationalRelationTypes {
        get => interOrganizationalRelationTypes;
        init {
            if (value is not null) {
                interOrganizationalRelationTypes = value;
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


}
