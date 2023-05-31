namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Document.ToUpdate), TypeInfoPropertyName = "DocumentToUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingDocumentJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(Document.ToCreate))]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForCreate")]
public partial class NewDocumentJsonContext : JsonSerializerContext { }
[JsonSerializable(typeof(DocumentDetails))]
public partial class DocumentDetailsJsonContext : JsonSerializerContext { }

public abstract record Document : SimpleTextNode, ResolvedNode
{
    private Document() { }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public required DocumentDetails DocumentDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }

    public sealed record ToUpdate : Document, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record ToCreate : Document, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
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

