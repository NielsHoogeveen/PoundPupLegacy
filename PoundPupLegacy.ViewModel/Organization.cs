namespace PoundPupLegacy.ViewModel;

public record Organization : Nameable, Documentable, Locatable
{
    public required string Description { get; init; }
    public required int Id { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private Link[] tags = Array.Empty<Link>();
    public Link[] Tags {
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
    public required Link[] BreadCrumElements { get; init; }

    private DocumentListItem[] documents = Array.Empty<DocumentListItem>();
    public required DocumentListItem[] Documents {
        get => documents;
        init {
            if (value is not null) {
                documents = value;
            }
        }
    }

    public string? WebsiteUrl { get; init; }
    public string? EmailAddress { get; init; }
    public DateTime? Established { get; init; }
    public DateTime? Terminated { get; init; }

    private Link[] organizationTypes = Array.Empty<Link>();
    public Link[] OrganizationTypes {
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

    private Link[] subTopics = Array.Empty<Link>();
    public Link[] SubTopics {
        get => subTopics;
        init {
            if (value is not null) {
                subTopics = value;
            }
        }
    }

    private Link[] superTopics = Array.Empty<Link>();
    public Link[] SuperTopics {
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
