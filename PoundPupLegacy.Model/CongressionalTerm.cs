namespace PoundPupLegacy.CreateModel;

public interface CongressionalTerm : Documentable
{
    List<CongressionalTermPoliticalPartyAffiliation> PartyAffiliations { get; }
}
