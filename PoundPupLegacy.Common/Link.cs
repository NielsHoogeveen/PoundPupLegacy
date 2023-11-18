namespace PoundPupLegacy.Common;

public record LinkBase: Link
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}
public interface Link
{
    string Path { get; }
    string Title { get; }

}
