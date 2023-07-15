namespace PoundPupLegacy.DomainModel;

public interface SearchableToUpdate : Searchable, NodeToUpdate
{
}
public interface SearchableToCreate : Searchable, NodeToCreate
{
}
public interface Searchable : Node
{
}
