namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableCongressionalTerm : CongressionalTerm, ImmediatelyIdentifiableDocumentable
{
}
public interface EventuallyIdentifiableCongressionalTerm : CongressionalTerm, EventuallyIdentifiableDocumentable
{
}
public interface CongressionalTerm : Documentable
{
    List<NewCongressionalTermPoliticalPartyAffiliation> PartyAffiliations { get; }
}
public abstract record NewCongressionalTermBase: NewNodeBase, EventuallyIdentifiableCongressionalTerm
{
    public required List<NewCongressionalTermPoliticalPartyAffiliation> PartyAffiliations { get; init; }
}
public abstract record ExistingCongressionalTermBase : ExistingNodeBase, ImmediatelyIdentifiableCongressionalTerm
{
    public required List<NewCongressionalTermPoliticalPartyAffiliation> PartyAffiliations { get; init; }
}