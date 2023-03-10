namespace PoundPupLegacy.ViewModel;

public record PartyPoliticalEntityRelation
{
    public required Link Party { get; init; }

    public required Link PoliticalEntity { get; init; }

    public required Link PartyPoliticalEntityRelationType { get; init; }

    public  DateTime? DateFrom { get; init; }

    public DateTime? DateTo { get; init; }

    public Link? DocumentProof { get; init; }
}
