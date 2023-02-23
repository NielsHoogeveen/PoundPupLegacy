namespace PoundPupLegacy.ViewModel;

public record CongressionalTerm
{
    public required string MemberType { get; init; }

    public required Link State { get; init; }
    public required DateTime? DateFrom { get; init; }
    public required DateTime? DateTo { get; init; }
}
