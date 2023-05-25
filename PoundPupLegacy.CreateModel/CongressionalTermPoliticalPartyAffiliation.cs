namespace PoundPupLegacy.CreateModel;

public sealed record NewCongressionalTermPoliticalPartyAffiliation : NewNodeBase, EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation
{
    public required int? CongressionalTermId { get; set; }

    public required int PoliticalPartyAffiliationId { get; init; }

    public required DateTimeRange DateTimeRange { get; init; }
}
public sealed record ExistingCongressionalTermPoliticalPartyAffiliation : ExistingNodeBase, ImmediatelyIdentifiableCongressionalTermPoliticalPartyAffiliation
{
    public required int CongressionalTermId { get; set; }

    public required int PoliticalPartyAffiliationId { get; init; }

    public required DateTimeRange DateTimeRange { get; init; }
}

public interface ImmediatelyIdentifiableCongressionalTermPoliticalPartyAffiliation : CongressionalTermPoliticalPartyAffiliation, ImmediatelyIdentifiableDocumentable, ImmediatelyIdentifiableSearchable
{
    int CongressionalTermId { get; }

}
public interface EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation: CongressionalTermPoliticalPartyAffiliation, EventuallyIdentifiableDocumentable, EventuallyIdentifiableSearchable
{
    int? CongressionalTermId { get; }
}
public interface CongressionalTermPoliticalPartyAffiliation: Documentable, Searchable
{

    int PoliticalPartyAffiliationId { get; }

    DateTimeRange DateTimeRange { get; }

}