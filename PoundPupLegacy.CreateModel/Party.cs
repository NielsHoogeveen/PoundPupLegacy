namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableParty : Party, ImmediatelyIdentifiableDocumentable, ImmediatelyIdentifiableLocatable, ImmediatelyIdentifiableNameable
{
    List<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> PartyPoliticalEntityRelationsToAdd { get; }
    List<ImmediatelyIdentifiablePartyPoliticalEntityRelation> PartyPoliticalEntityRelationsToUpdate { get; }

}
public interface EventuallyIdentifiableParty: Party, EventuallyIdentifiableDocumentable, EventuallyIdentifiableLocatable, EventuallyIdentifiableNameable 
{
    List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty> PartyPoliticalEntityRelations { get; }
}
public interface Party : Documentable, Locatable, Nameable
{
}

public abstract record NewPartyBase : NewLocatableBase, EventuallyIdentifiableParty
{
    public required List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty> PartyPoliticalEntityRelations { get; init; }
}
public abstract record ExistingPartyBase : ExistingLocatableBase, ImmediatelyIdentifiableParty
{
    public required List<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> PartyPoliticalEntityRelationsToAdd { get; init; }
    public required List<ImmediatelyIdentifiablePartyPoliticalEntityRelation> PartyPoliticalEntityRelationsToUpdate { get; init; }

}

