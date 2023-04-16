namespace PoundPupLegacy.ViewModel.Models;

public record BindingCountry : TopLevelCountry
{
    public required int NodeId { get; init; }
    public required string Description { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }
    public required string ISO3166_1_Code { get; init; }
    public required Link GlobalRegion { get; init; }

    private Link[] boundCountries = Array.Empty<Link>();
    public required Link[] BoundCountries
    {
        get => boundCountries;
        init
        {
            if (value is not null)
            {
                boundCountries = value;
            }

        }
    }

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
    private OrganizationTypeWithOrganizations[] organizationTypes = Array.Empty<OrganizationTypeWithOrganizations>();
    public required OrganizationTypeWithOrganizations[] OrganizationTypes
    {
        get => organizationTypes;
        init
        {
            if (value is not null)
            {
                organizationTypes = value;
            }
        }
    }
    private SubdivisionType[] subdivisionTypes = Array.Empty<SubdivisionType>();
    public required SubdivisionType[] SubdivisionTypes
    {
        get => subdivisionTypes;
        init
        {
            if (value is not null)
            {
                subdivisionTypes = value;
            }
        }
    }
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
    public required Image FlagImage { get; init; }
}
