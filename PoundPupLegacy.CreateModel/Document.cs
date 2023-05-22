namespace PoundPupLegacy.CreateModel;

public sealed record NewDocument : NewSimpleTextNodeBase, EventuallyIdentifiableDocument
{
    public required FuzzyDate? PublicationDate { get; init; }
    public required string? SourceUrl { get; init; }
    public required int? DocumentTypeId { get; init; }
    public required List<int> Documentables { get; init; }
}
public sealed record ExistingDocument : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableDocument
{
    public required FuzzyDate? PublicationDate { get; init; }
    public required string? SourceUrl { get; init; }
    public required int? DocumentTypeId { get; init; }
    public required List<int> Documentables { get; init; }
}
public interface ImmediatelyIdentifiableDocument : Document, ImmediatelyIdentifiableSimpleTextNode
{

}
public interface EventuallyIdentifiableDocument : Document, EventuallyIdentifiableSimpleTextNode
{

}
public interface Document : SimpleTextNode
{
    FuzzyDate? PublicationDate { get;  }
    string? SourceUrl { get; }
    int? DocumentTypeId { get; }
    List<int> Documentables { get; }
}
