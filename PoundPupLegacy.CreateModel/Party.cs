namespace PoundPupLegacy.CreateModel;
public interface PartyToUpdate : Party, DocumentableToUpdate, LocatableToUpdate, NameableToUpdate
{
}
public interface PartyToCreate: Party, DocumentableToCreate, LocatableToCreate, NameableToCreate 
{
}
public interface Party : Documentable, Locatable, Nameable
{
}

