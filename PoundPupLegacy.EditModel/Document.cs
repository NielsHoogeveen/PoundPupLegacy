namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Document.ExistingDocument))]
public partial class ExistingDocumentJsonContext : JsonSerializerContext { }
[JsonSerializable(typeof(Document.NewDocument))]
public partial class NewDocumentJsonContext : JsonSerializerContext { }
[JsonSerializable(typeof(DocumentDetails))]
public partial class DocumentDetailsJsonContext : JsonSerializerContext { }

public abstract record Document : SimpleTextNode, ResolvedNode
{
    private Document() { }
    public abstract T Match<T>(Func<ExistingDocument, T> existingItem, Func<NewDocument, T> newItem);
    public abstract void Match(Action<ExistingDocument> existingItem, Action<NewDocument> newItem);
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public required DocumentDetails DocumentDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }

    public sealed record ExistingDocument : Document, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingDocument, T> existingItem, Func<NewDocument, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingDocument> existingItem, Action<NewDocument> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record NewDocument : Document, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override T Match<T>(Func<ExistingDocument, T> existingItem, Func<NewDocument, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingDocument> existingItem, Action<NewDocument> newItem)
        {
            newItem(this);
        }
    }
}

public sealed record DocumentDetails
{
    public string? SourceUrl { get; set; }

    public int DocumentTypeId { get; set; }

    public DateTime? PublicationDateFrom { get; set; }

    public DateTime? PublicationDateTo { get; set; }

    private bool _publishedSet;

    private FuzzyDate? _published;

    public FuzzyDate? Published {
        get {
            if (!_publishedSet) {
                if (PublicationDateFrom is not null && PublicationDateTo is not null) {
                    var dateTimeRange = new DateTimeRange(PublicationDateFrom, PublicationDateTo);
                    if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                        _published = result;
                    }
                }
                else {
                    _published = null;
                }
                _publishedSet = true;
            }
            return _published;
        }
        set {
            _published = value;
        }
    }

    public required DocumentType[] DocumentTypes { get; init; }
}

