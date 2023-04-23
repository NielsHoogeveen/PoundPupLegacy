namespace PoundPupLegacy.ViewModel.Models;

public record Organization : Nameable, Documentable, Locatable
{
    public required string Description { get; init; }
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private BasicLink[] tags = Array.Empty<BasicLink>();
    public BasicLink[] Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }

        }
    }
    private CommentListItem[] commentListItems = Array.Empty<CommentListItem>();
    public CommentListItem[] CommentListItems {
        get => commentListItems;
        init {
            if (value is not null) {
                commentListItems = value;
            }
        }
    }

    public Comment[] Comments => this.GetComments();
    public required BasicLink[] BreadCrumElements { get; init; }

    private DocumentListEntry[] documents = Array.Empty<DocumentListEntry>();
    public required DocumentListEntry[] Documents {
        get => documents;
        init {
            if (value is not null) {
                documents = value;
            }
        }
    }

    public string? WebsiteUrl { get; init; }
    public string? EmailAddress { get; init; }

    public DateTime? EstablishmentDateFrom { get; init; }
    public DateTime? EstablishmentDateTo { get; init; }
    public DateTime? TerminationDateFrom { get; init; }
    public DateTime? TerminationDateTo { get; init; }

    public FuzzyDate? Establishment {
        get {
            if (EstablishmentDateFrom is not null && EstablishmentDateTo is not null) {
                return new DateTimeRange(EstablishmentDateFrom, EstablishmentDateTo).ToFuzzyDate();
            }
            return null;
        }
    }
    public FuzzyDate? Termination {
        get {
            if (TerminationDateFrom is not null && TerminationDateTo is not null) {
                return new DateTimeRange(TerminationDateFrom, TerminationDateTo).ToFuzzyDate();
            }
            return null;
        }
    }

    private BasicLink[] organizationTypes = Array.Empty<BasicLink>();
    public BasicLink[] OrganizationTypes {
        get => organizationTypes;
        init {
            if (value is not null) {
                organizationTypes = value;
            }
        }
    }

    private Location[] _locations = Array.Empty<Location>();
    public Location[] Locations {
        get => _locations;
        init {
            if (value is not null) {
                _locations = value;
            }
        }
    }

    private BasicLink[] subTopics = Array.Empty<BasicLink>();
    public BasicLink[] SubTopics {
        get => subTopics;
        init {
            if (value is not null) {
                subTopics = value;
            }
        }
    }

    private BasicLink[] superTopics = Array.Empty<BasicLink>();
    public BasicLink[] SuperTopics {
        get => superTopics;
        init {
            if (value is not null) {
                superTopics = value;
            }
        }
    }
    private InterOrganizationalRelation[] interOrganizationalRelations = Array.Empty<InterOrganizationalRelation>();
    public InterOrganizationalRelation[] InterOrganizationalRelations {
        get => interOrganizationalRelations;
        init {
            if (value is not null) {
                interOrganizationalRelations = value;
            }
        }
    }
    private PartyCaseType[] partyCaseTypes = Array.Empty<PartyCaseType>();
    public PartyCaseType[] PartyCaseTypes {
        get => partyCaseTypes;
        init {
            if (value is not null) {
                partyCaseTypes = value;
            }
        }
    }
    private PersonOrganizationRelation[] personOrganizationRelations = Array.Empty<PersonOrganizationRelation>();
    public PersonOrganizationRelation[] PersonOrganizationRelations {
        get => personOrganizationRelations;
        init {
            if (value is not null) {
                personOrganizationRelations = value;
            }
        }
    }
    private PartyPoliticalEntityRelation[] partyPoliticalEntityRelations = Array.Empty<PartyPoliticalEntityRelation>();
    public PartyPoliticalEntityRelation[] PartyPoliticalEntityRelations {
        get => partyPoliticalEntityRelations;
        init {
            if (value is not null) {
                partyPoliticalEntityRelations = value;
            }
        }
    }
    private File[] _files = Array.Empty<File>();
    public required File[] Files {
        get => _files;
        init {
            if (value is not null) {
                _files = value;
            }
        }
    }

}
