using System.Text.Encodings.Web;

namespace PoundPupLegacy.ViewModel;

public record class Document : Node
{
    public required int Id { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required DateTime? DateTime { get; init; }
    public required DateTime? DateTimeFrom { get; init; }
    public required DateTime? DateTimeTo { get; init; }
    public required Link? DocumentType { get; init; }
    public string? PublicationDate { 
        get
        {
            if(DateTime.HasValue)
            {
                return DateTime.Value.ToString("yyyy MMMM dd");
            }
            if(DateTimeFrom.HasValue && DateTimeTo.HasValue)
            {
                if(DateTimeFrom.Value.Month == DateTimeTo.Value.Month)
                {
                    return DateTimeFrom.Value.ToString("yyyy MMMM");
                }
                else
                {
                    return DateTimeFrom.Value.ToString("yyyy");
                }
            }
            return null;
        }    
    }

    public required string? SourceUrl { get; init; }
    public string? SourceUrlHost => SourceUrl is null? null: new Uri(SourceUrl).Host;
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private Link[] tags = Array.Empty<Link>();
    public Link[] Tags
    {
        get => tags;
        init
        {
            if(value is not null)
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

    private Comment[] comments = Array.Empty<Comment>();
    public Comment[] Comments
    {
        get => comments;
        init
        {
            if(value is not null)
            {
                comments = value;
            }
        }
    }

    public required Link[] BreadCrumElements { get; init; }

}
