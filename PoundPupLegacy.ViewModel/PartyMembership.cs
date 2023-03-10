namespace PoundPupLegacy.ViewModel;

public record PartyMembership
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}
