namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableParty : Party, ImmediatelyIdentifiableDocumentable, ImmediatelyIdentifiableLocatable, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableParty: Party, EventuallyIdentifiableDocumentable, EventuallyIdentifiableLocatable, EventuallyIdentifiableNameable
{
}
public interface Party : Documentable, Locatable, Nameable
{
}
