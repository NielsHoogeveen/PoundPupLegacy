namespace PoundPupLegacy.ViewModel;

public record PartyPoliticalEntityRelation
{
    public required Link Party { get; init; }

    public required Link PoliticalEntity { get; init; }

    public required Link PartyPoliticalEntityRelationType { get; init; }

    public required DateTime? DateFrom { get; init; }

    public required DateTime? DateTo { get; init; }

    public required Link? DocumentProof { get; init; }
}
