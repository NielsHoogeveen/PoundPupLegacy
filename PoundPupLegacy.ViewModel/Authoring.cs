namespace PoundPupLegacy.ViewModel;

public record Authoring
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
}
