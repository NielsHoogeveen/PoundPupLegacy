namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CongressionalTerm))]
public partial class CongressionalTermJsonContext : JsonSerializerContext { }

public record CongressionalTerm
{
    public required string MemberType { get; init; }

    public required BasicLink State { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}
