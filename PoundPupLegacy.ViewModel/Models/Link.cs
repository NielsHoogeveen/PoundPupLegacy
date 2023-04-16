namespace PoundPupLegacy.ViewModel.Models;

public record Link
{
    public required string Path { get; init; }
    public required string Name { get; init; }
}
