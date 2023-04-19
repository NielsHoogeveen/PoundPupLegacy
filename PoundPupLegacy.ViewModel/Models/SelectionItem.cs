namespace PoundPupLegacy.ViewModel.Models;

public record SelectionItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    private bool _selected;
    public bool Selected {
        get => _selected;
        set {
            _selected = value;
        }
    }
}
