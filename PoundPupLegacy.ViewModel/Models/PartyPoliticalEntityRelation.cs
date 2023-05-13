namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PartyPoliticalEntityRelation))]
public partial class PartyPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public record PartyPoliticalEntityRelation
{
    public required BasicLink Party { get; init; }

    public required BasicLink PoliticalEntity { get; init; }

    public required BasicLink PartyPoliticalEntityRelationType { get; init; }

    public DateTime? DateFrom { get; init; }

    public DateTime? DateTo { get; init; }

    public BasicLink? ProofDocument { get; init; }
}
