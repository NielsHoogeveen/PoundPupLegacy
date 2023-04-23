namespace PoundPupLegacy.ViewModel.Models;

public record FormalSubdivision : Subdivision
{
    public required string Description { get; init; }
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    public required BasicLink Country { get; init; }
    public required string ISO3166_2_Code { get; init; }

    private BasicLink[] tags = Array.Empty<BasicLink>();
    public required BasicLink[] Tags {
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

    public Comment[] Comments => this.GetComments(); public required BasicLink[] BreadCrumElements { get; init; }


    private DocumentListEntry[] documents = Array.Empty<DocumentListEntry>();
    public required DocumentListEntry[] Documents {
        get => documents;
        init {
            if (value is not null) {
                documents = value;
            }
        }
    }
    public required OrganizationTypeWithOrganizations[] OrganizationTypes { get; init; }

    private BasicLink[] subTopics = Array.Empty<BasicLink>();
    public required BasicLink[] SubTopics {
        get => subTopics;
        init {
            if (value is not null) {
                subTopics = value;
            }
        }
    }

    private BasicLink[] superTopics = Array.Empty<BasicLink>();
    public required BasicLink[] SuperTopics {
        get => superTopics;
        init {
            if (value is not null) {
                superTopics = value;
            }
        }
    }
    public required SubdivisionType[] SubdivisionTypes { get; init; }

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
