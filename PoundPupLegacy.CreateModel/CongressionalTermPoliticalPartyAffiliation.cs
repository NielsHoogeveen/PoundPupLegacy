namespace PoundPupLegacy.CreateModel;

public abstract record CongressionalTermPoliticalPartyAffiliation : Documentable, Searchable
{
    private CongressionalTermPoliticalPartyAffiliation() { }
    public abstract CongressionalTermPoliticalPartyAffiliationDetails CongressionalTermPoliticalPartyAffiliationDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }

    public sealed record ToCreateForExistingTerm : CongressionalTermPoliticalPartyAffiliation, DocumentableToCreate, SearchableToCreate
    {
        public required CongressionalTermPoliticalPartyAffiliationDetails.CongressionalTermPoliticalPartyAffiliationDetailsResolved CongressionalTermPoliticalPartyAffiliationDetailsResolved { get; init; }
        public override CongressionalTermPoliticalPartyAffiliationDetails CongressionalTermPoliticalPartyAffiliationDetails => CongressionalTermPoliticalPartyAffiliationDetailsResolved;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record ToCreateForNewTerm : CongressionalTermPoliticalPartyAffiliation, DocumentableToCreate, SearchableToCreate
    {
        public required CongressionalTermPoliticalPartyAffiliationDetails.CongressionalTermPoliticalPartyAffiliationDetailsUnresolved CongressionalTermPoliticalPartyAffiliationDetailsUnresolved { get; init; }
        public override CongressionalTermPoliticalPartyAffiliationDetails CongressionalTermPoliticalPartyAffiliationDetails => CongressionalTermPoliticalPartyAffiliationDetailsUnresolved;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public ToCreateForExistingTerm ResolveCongressionalTerm(int congressionalTermId)
        {
            return new() {
                CongressionalTermPoliticalPartyAffiliationDetailsResolved = CongressionalTermPoliticalPartyAffiliationDetailsUnresolved.ResolveCongressionalTerm(congressionalTermId),
                IdentificationForCreate = IdentificationForCreate,
                NodeDetailsForCreate = NodeDetailsForCreate
            };
        }
    }
    public sealed record ToUpdate : CongressionalTermPoliticalPartyAffiliation, DocumentableToUpdate, SearchableToUpdate
    {
        public required CongressionalTermPoliticalPartyAffiliationDetails.CongressionalTermPoliticalPartyAffiliationDetailsResolved CongressionalTermPoliticalPartyAffiliationDetailsResolved { get; init; }
        public override CongressionalTermPoliticalPartyAffiliationDetails CongressionalTermPoliticalPartyAffiliationDetails => CongressionalTermPoliticalPartyAffiliationDetailsResolved;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}

public abstract record CongressionalTermPoliticalPartyAffiliationDetails
{
    private CongressionalTermPoliticalPartyAffiliationDetails() { }
    
    public required int PoliticalPartyAffiliationId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

    public sealed record CongressionalTermPoliticalPartyAffiliationDetailsResolved: CongressionalTermPoliticalPartyAffiliationDetails
    {
        public required int CongressionalTermId { get; set; }
    }
    public sealed record CongressionalTermPoliticalPartyAffiliationDetailsUnresolved: CongressionalTermPoliticalPartyAffiliationDetails
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