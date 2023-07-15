namespace PoundPupLegacy.DomainModel;
public interface ISOCodedFirstLevelSubdivisionToUpdate : ISOCodedFirstLevelSubdivision, FirstLevelSubdivisionToUpdate, ISOCodedSubdivisionToUpdate
{
}
public interface ISOCodedFirstLevelSubdivisionToCreate : ISOCodedFirstLevelSubdivision, FirstLevelSubdivisionToCreate, ISOCodedSubdivisionToCreate
{
}
public interface ISOCodedFirstLevelSubdivision : FirstLevelSubdivision, ISOCodedSubdivision
{
}
