namespace PoundPupLegacy.CreateModel;

public sealed record Term : Identifiable
{
    public required int? Id { get; set; }
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
    public required int NameableId { get; init; }
}
