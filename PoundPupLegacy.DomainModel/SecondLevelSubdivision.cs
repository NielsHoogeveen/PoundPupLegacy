﻿namespace PoundPupLegacy.DomainModel;
public interface SecondLevelSubdivisionToUpdate : SecondLevelSubdivision, ISOCodedSubdivisionToUpdate, BottomLevelSubdivisionToUpdate
{
}
public interface SecondLevelSubdivisionToCreate : SecondLevelSubdivision, ISOCodedSubdivisionToCreate, BottomLevelSubdivisionToCreate
{
}
public interface SecondLevelSubdivision : ISOCodedSubdivision, BottomLevelSubdivision
{
}