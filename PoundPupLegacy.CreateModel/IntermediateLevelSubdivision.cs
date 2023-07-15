namespace PoundPupLegacy.DomainModel;

public interface IntermediateLevelSubdivisionToUpdate : IntermediateLevelSubdivision, FirstLevelSubdivisionToUpdate
{
}
public interface IntermediateLevelSubdivisionToCreate : IntermediateLevelSubdivision, FirstLevelSubdivisionToCreate
{
}
public interface IntermediateLevelSubdivision : FirstLevelSubdivision
{
}
