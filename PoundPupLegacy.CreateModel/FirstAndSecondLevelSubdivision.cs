namespace PoundPupLegacy.CreateModel;
public interface FirstAndSecondLevelSubdivisionToUpdate : FirstAndSecondLevelSubdivision, ISOCodedSubdivisionToUpdate, SecondLevelSubdivisionToUpdate
{
}

public interface FirstAndSecondLevelSubdivisionToCreate : FirstAndSecondLevelSubdivision, ISOCodedSubdivisionToCreate, SecondLevelSubdivisionToCreate 
{ 
}

public interface FirstAndSecondLevelSubdivision : ISOCodedFirstLevelSubdivision, SecondLevelSubdivision
{
}
