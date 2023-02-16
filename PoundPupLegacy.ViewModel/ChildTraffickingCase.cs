namespace PoundPupLegacy.ViewModel;

public record ChildTraffickingCase : Case
{
    public required string Description { get; init; }
    public required int Id { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

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

    private Comment[] comments = Array.Empty<Comment>();
    public required Comment[] Comments
    {
        get => comments;
        init
        {
            if (value is not null)
            {
                comments = value;
            }
        }
    }
    public required Link[] BreadCrumElements { get; init; }

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

    private Location[] _locations = Array.Empty<Location>();
    public required Location[] Locations
    {
        get => _locations;
        init
        {
            if (value is not null)
            {
                _locations = value;
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

    private CaseParties[] _caseParties = Array.Empty<CaseParties>();
    public required CaseParties[] CaseParties
    {
        get => _caseParties;
        init
        {
            if (value is not null)
            {
                _caseParties = value;
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
}
