namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PartyMembership))]
public partial class PartyMembershipJsonContext : JsonSerializerContext { }

public sealed record PartyMembership : LinkBase
{
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}
