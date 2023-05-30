namespace PoundPupLegacy.CreateModel;

public interface CongressionalTermToUpdate : CongressionalTerm, DocumentableToUpdate
{
    CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetails { get; }
}
public interface CongressionalTermToCreate : CongressionalTerm, DocumentableToCreate
{
    CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetails { get; }
}
public interface CongressionalTerm : Documentable
{
}
public abstract record CongressionalTermDetails
{
    public sealed record CongressionalTermDetailsForCreate: CongressionalTermDetails
    {
        public required List<CongressionalTermPoliticalPartyAffiliation.ToCreateForNewTerm> PartyAffiliations { get; init; }
    }
    public sealed record CongressionalTermDetailsForUpdate: CongressionalTermDetails
    {

    }
}
