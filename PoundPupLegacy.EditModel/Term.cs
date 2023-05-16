namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Term))]
public partial class TermJsonContext : JsonSerializerContext { }

public sealed record Term
{
    public required int? Id { get; set; }

    public required string Name { get; set; }

    public required int? NodeId { get; set; }

    public required int VocabularyId { get; set; }
}
