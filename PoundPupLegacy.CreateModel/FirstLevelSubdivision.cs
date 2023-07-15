namespace PoundPupLegacy.DomainModel;

public interface FirstLevelSubdivisionToUpdate : FirstLevelSubdivision, SubdivisionToUpdate
{
}
public interface FirstLevelSubdivisionToCreate : FirstLevelSubdivision, SubdivisionToCreate
{
}
public interface FirstLevelSubdivision : Subdivision
{
}
