namespace PoundPupLegacy.CreateModel;

public interface CongressionalTermToUpdate : CongressionalTerm, DocumentableToUpdate
{
}
public interface CongressionalTermToCreate : CongressionalTerm, DocumentableToCreate
{
}
public interface CongressionalTerm : Documentable
{
    CongressionalTermDetails CongressionalTermDetails { get; }
}
public record CongressionalTermDetails
{
    public required List<CongressionalTermPoliticalPartyAffiliation.CongressionalTermPoliticalPartyAffiliationToCreate> PartyAffiliations { get; init; }
}
