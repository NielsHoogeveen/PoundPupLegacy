namespace PoundPupLegacy.ViewModel;

public record SelectionItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public bool Selected { get; set; }
}
