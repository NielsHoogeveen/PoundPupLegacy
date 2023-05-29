namespace PoundPupLegacy.CreateModel;

public abstract record CongressionalTermPoliticalPartyAffiliation : Documentable, Searchable
{
    private CongressionalTermPoliticalPartyAffiliation() { }
    public required CongressionalTermPoliticalPartyAffiliationDetails CongressionalTermPoliticalPartyAffiliationDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<CongressionalTermPoliticalPartyAffiliationToCreate, T> create, Func<CongressionalTermPoliticalPartyAffiliationToUpdate, T> update);
    public abstract void Match(Action<CongressionalTermPoliticalPartyAffiliationToCreate> create, Action<CongressionalTermPoliticalPartyAffiliationToUpdate> update);

    public sealed record CongressionalTermPoliticalPartyAffiliationToCreate : CongressionalTermPoliticalPartyAffiliation, DocumentableToCreate, SearchableToCreate
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<CongressionalTermPoliticalPartyAffiliationToCreate, T> create, Func<CongressionalTermPoliticalPartyAffiliationToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<CongressionalTermPoliticalPartyAffiliationToCreate> create, Action<CongressionalTermPoliticalPartyAffiliationToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record CongressionalTermPoliticalPartyAffiliationToUpdate : CongressionalTermPoliticalPartyAffiliation, DocumentableToUpdate, SearchableToUpdate
    {
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<CongressionalTermPoliticalPartyAffiliationToCreate, T> create, Func<CongressionalTermPoliticalPartyAffiliationToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<CongressionalTermPoliticalPartyAffiliationToCreate> create, Action<CongressionalTermPoliticalPartyAffiliationToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record CongressionalTermPoliticalPartyAffiliationDetails
{
    public required int CongressionalTermId { get; set; }
    public required int PoliticalPartyAffiliationId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

}