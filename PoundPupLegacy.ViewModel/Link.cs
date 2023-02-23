namespace PoundPupLegacy.ViewModel;

public record Link
{
    public required string Path { get; init; }
    public required string Name { get; init; }
}
