namespace PoundPupLegacy.EditModel;


public interface Document: SimpleTextNode
{
    string? SourceUrl { get; set; }

    int DocumentTypeId { get; set; }

    DateTime? PublicationDateFrom { get; set; }

    DateTime? PublicationDateTo { get; set; }

    public FuzzyDate? Published { get; set; }

    DocumentType[] DocumentTypes { get; }
}


[JsonSerializable(typeof(ExistingDocument))]
public partial class ExistingDocumentJsonContext : JsonSerializerContext { }

public record ExistingDocument: DocumentBase, ExistingNode
{
    public int NodeId { get; set; }

    public int UrlId { get; set; }

}
[JsonSerializable(typeof(NewDocument))]
public partial class NewDocumentJsonContext : JsonSerializerContext { }

public record NewDocument : DocumentBase, NewNode
{
}

public record DocumentBase : SimpleTextNodeBase, Document
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
