namespace PoundPupLegacy.EditModel;

public record Person : Party
{
    public int? NodeId { get; init; }
    public int? UrlId { get; set; }
    public required string NodeTypeName { get; set; }
    public required string Title { get; set; }
    public required int PublisherId { get; set; }
    public required int OwnerId { get; set; }
    public required string Description { get; set; }

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

    private List<CountryListItem> countries = new();

    public List<CountryListItem> Countries {
        get => countries;
        init {
            if (value is not null) {
                countries = value;
            }
        }
    }

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
