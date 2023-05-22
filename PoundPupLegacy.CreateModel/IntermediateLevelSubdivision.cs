﻿namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableIntermediateLevelSubdivision : IntermediateLevelSubdivision, ImmediatelyIdentifiableFirstLevelSubdivision
{

}
public interface EventuallyIdentifiableIntermediateLevelSubdivision : IntermediateLevelSubdivision, EventuallyIdentifiableFirstLevelSubdivision
{

}
public interface IntermediateLevelSubdivision : FirstLevelSubdivision
{
}
