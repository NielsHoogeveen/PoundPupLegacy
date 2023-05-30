namespace PoundPupLegacy.CreateModel;

public interface CongressionalTermToUpdate : CongressionalTerm, DocumentableToUpdate
{
    CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetailsForUpdate { get; }
}
public interface CongressionalTermToCreate : CongressionalTerm, DocumentableToCreate
{
    CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetailsForCreate { get; }
}
public interface CongressionalTerm : Documentable
{
    CongressionalTermDetails CongressionalTermDetails { get; }
}
public abstract record CongressionalTermDetails
{
    public sealed record CongressionalTermDetailsForCreate: CongressionalTermDetails
    {
        public required List<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreateForNewTerm> PartyAffiliations { get; init; }
    }
    public sealed record CongressionalTermDetailsForUpdate: CongressionalTermDetails
    {

    }
}
