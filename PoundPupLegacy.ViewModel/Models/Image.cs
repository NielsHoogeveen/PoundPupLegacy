namespace PoundPupLegacy.ViewModel.Models;

public record Image
{
    public required string FilePath { get; init; }

    public required string? Label { get; init; }
}
