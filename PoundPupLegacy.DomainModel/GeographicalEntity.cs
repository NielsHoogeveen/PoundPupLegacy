namespace PoundPupLegacy.DomainModel;

public interface GeographicalEntityToUpdate : GeographicalEntity, DocumentableToUpdate, NameableToUpdate
{
}
public interface GeographicalEntityToCreate : GeographicalEntity, DocumentableToCreate, NameableToCreate
{
}
public interface GeographicalEntity : Documentable, Nameable
{
}
