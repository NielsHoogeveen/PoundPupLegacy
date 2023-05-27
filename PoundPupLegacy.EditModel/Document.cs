namespace PoundPupLegacy.EditModel;


public interface Document : SimpleTextNode, ResolvedNode
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

public sealed record ExistingDocument : ExistingSimpleTextNodeBase, Document 
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
[JsonSerializable(typeof(NewDocument))]
public partial class NewDocumentJsonContext : JsonSerializerContext { }

public sealed record NewDocument : NewSimpleTextNodeBase, ResolvedNewNode, Document
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

