namespace PoundPupLegacy.ViewModel.Models;

public record PartyMembership: Link
{
    public required string Title { get; init; }
    public required string Path { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}
