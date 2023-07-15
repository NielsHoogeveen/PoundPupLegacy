namespace PoundPupLegacy.DomainModel;
public interface BottomLevelSubdivisionToUpdate : BottomLevelSubdivision, SubdivisionToUpdate
{
}

public interface BottomLevelSubdivisionToCreate : BottomLevelSubdivision, SubdivisionToCreate
{
}
public interface BottomLevelSubdivision : Subdivision
{
}
