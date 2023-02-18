namespace PoundPupLegacy.ViewModel;

public record BoundCountry : Country
{
    public required string Description { get; init; }
    public required int Id { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }
    
    public required Link BindingCountry { get; init; }

    private Link[] tags = Array.Empty<Link>();
    public required Link[] Tags
    {
        get => tags;
        init
        {
            if (value is not null)
            {
                tags = value;
            }

        }
    }

    private CommentListItem[] commentListItems = Array.Empty<CommentListItem>();
    public CommentListItem[] CommentListItems
    {
        get => commentListItems;
        init
        {
            if (value is not null)
            {
                commentListItems = value;
            }
        }
    }

    public Comment[] Comments => this.GetComments(); public required Link[] BreadCrumElements { get; init; }
    
    public required AdoptionImports AdoptionImports { get; init; }

    private DocumentListItem[] documents = Array.Empty<DocumentListItem>();
    public required DocumentListItem[] Documents
    {
        get => documents;
        init
        {
            if (value is not null)
            {
                documents = value;
            }
        }
    }
    public required OrganizationTypeWithOrganizations[] OrganizationTypes { get; init; }

    private Link[] subTopics = Array.Empty<Link>();
    public required Link[] SubTopics
    {
        get => subTopics;
        init
        {
            if (value is not null)
            {
                subTopics = value;
            }
        }
    }

    private Link[] superTopics = Array.Empty<Link>();
    public required Link[] SuperTopics
    {
        get => superTopics;
        init
        {
            if (value is not null)
            {
                superTopics = value;
            }
        }
    }
    public required SubdivisionType[] SubdivisionTypes { get; init; }

    private File[] _files = Array.Empty<File>();
    public required File[] Files
    {
        get => _files;
        init
        {
            if (value is not null)
            {
                _files = value;
            }
        }
    }

}
