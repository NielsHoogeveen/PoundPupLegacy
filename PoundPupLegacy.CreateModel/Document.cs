namespace PoundPupLegacy.CreateModel;

public sealed record NewDocument : NewSimpleTextNodeBase, EventuallyIdentifiableDocument
{
    public required FuzzyDate? Published { get; init; }
    public required string? SourceUrl { get; init; }
    public required int? DocumentTypeId { get; init; }
    //public required List<int> Documentables { get; init; }
}
public sealed record ExistingDocument : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableDocument
{
    public required FuzzyDate? Published { get; init; }
    public required string? SourceUrl { get; init; }
    public required int DocumentTypeId { get; init; }
    //public required List<int> Documentables { get; init; }
}
public interface ImmediatelyIdentifiableDocument : Document, ImmediatelyIdentifiableSimpleTextNode
{
    int DocumentTypeId { get; }

}
public interface EventuallyIdentifiableDocument : Document, EventuallyIdentifiableSimpleTextNode
{
    int? DocumentTypeId { get; }
}
public interface Document : SimpleTextNode
{
    FuzzyDate? Published { get;  }
    string? SourceUrl { get; }
    //List<int> Documentables { get; }
}
