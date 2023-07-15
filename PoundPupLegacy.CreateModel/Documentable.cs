namespace PoundPupLegacy.DomainModel;
public interface DocumentableToUpdate : Documentable, SearchableToUpdate
{
}
public interface DocumentableToCreate : Documentable, SearchableToCreate
{
}
public interface Documentable : Searchable
{
}
