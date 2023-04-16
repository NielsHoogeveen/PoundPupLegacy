using PoundPupLegacy.Common;

namespace PoundPupLegacy.ViewModel.Models;

public record Document : Node
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public DateTime? PublicationDateFrom { get; set; }
    public DateTime? PublicationDateTo { get; set; }
    public FuzzyDate? Published
    {
        get
        {
            if (PublicationDateFrom is not null && PublicationDateTo is not null)
            {
                var dateTimeRange = new DateTimeRange(PublicationDateFrom, PublicationDateTo);
                if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result))
                {
                    return result;
                }
            }
            return null;
        }
    }

    public Link? DocumentType { get; init; }
    public string? SourceUrl { get; init; }
    public string? SourceUrlHost => SourceUrl is null ? null : new Uri(SourceUrl).Host;
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private Link[] tags = Array.Empty<Link>();
    public Link[] Tags
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
    private Link[] documentables = Array.Empty<Link>();
    public Link[] Documentables
    {
        get => documentables;
        init
        {
            if (value is not null)
            {
                documentables = value;
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

    public Comment[] Comments => this.GetComments();
    public required Link[] BreadCrumElements { get; init; }

    private File[] _files = Array.Empty<File>();

    public Document()
    {
    }

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
