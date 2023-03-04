namespace PoundPupLegacy.Model;

public interface CongressionalTerm : Documentable
{
    List<CongressionalTermPoliticalPartyAffiliation> PartyAffiliations { get; }
}
