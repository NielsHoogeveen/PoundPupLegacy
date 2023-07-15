namespace PoundPupLegacy.DomainModel;

public abstract record CongressionalTermPoliticalPartyAffiliation : Documentable, Searchable
{
    private CongressionalTermPoliticalPartyAffiliation() { }

    public sealed record ToCreateForExistingTerm : CongressionalTermPoliticalPartyAffiliation, DocumentableToCreate, SearchableToCreate
    {
        public required CongressionalTermPoliticalPartyAffiliationDetails.CongressionalTermPoliticalPartyAffiliationDetailsResolved CongressionalTermPoliticalPartyAffiliationDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }

    }
    public sealed record ToCreateForNewTerm : CongressionalTermPoliticalPartyAffiliation, DocumentableToCreate, SearchableToCreate
    {
        public required CongressionalTermPoliticalPartyAffiliationDetails.CongressionalTermPoliticalPartyAffiliationDetailsUnresolved CongressionalTermPoliticalPartyAffiliationDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public ToCreateForExistingTerm ResolveCongressionalTerm(int congressionalTermId)
        {
            return new() {
                CongressionalTermPoliticalPartyAffiliationDetails = CongressionalTermPoliticalPartyAffiliationDetails.ResolveCongressionalTerm(congressionalTermId),
                Identification = Identification,
                NodeDetails = NodeDetails
            };
        }
    }
    public sealed record ToUpdate : CongressionalTermPoliticalPartyAffiliation, DocumentableToUpdate, SearchableToUpdate
    {
        public required CongressionalTermPoliticalPartyAffiliationDetails.CongressionalTermPoliticalPartyAffiliationDetailsResolved CongressionalTermPoliticalPartyAffiliationDetails { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public abstract record CongressionalTermPoliticalPartyAffiliationDetails
{
    private CongressionalTermPoliticalPartyAffiliationDetails() { }

    public required int PoliticalPartyAffiliationId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

    public sealed record CongressionalTermPoliticalPartyAffiliationDetailsResolved : CongressionalTermPoliticalPartyAffiliationDetails
    {
        public required int CongressionalTermId { get; set; }
    }
    public sealed record CongressionalTermPoliticalPartyAffiliationDetailsUnresolved : CongressionalTermPoliticalPartyAffiliationDetails
    {
        public CongressionalTermPoliticalPartyAffiliationDetailsResolved ResolveCongressionalTerm(int congressionalTermId)
        {
            return new() {
                CongressionalTermId = congressionalTermId,
                PoliticalPartyAffiliationId = PoliticalPartyAffiliationId,
                DateTimeRange = DateTimeRange
            };
        }
    }

}