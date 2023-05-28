﻿namespace PoundPupLegacy.CreateModel;

public sealed record NewInformalIntermediateLevelSubdivision : NewSubdivisionBase, EventuallyIdentifiableInformalIntermediateLevelSubdivision
{
}
public sealed record ExistingInformalIntermediateLevelSubdivision : ExistingSubdivisionBase, ImmediatelyIdentifiableInformalIntermediateLevelSubdivision
{
}
public interface ImmediatelyIdentifiableInformalIntermediateLevelSubdivision : InformalIntermediateLevelSubdivision, IntermediateLevelSubdivisionToUpdate
{
}
public interface EventuallyIdentifiableInformalIntermediateLevelSubdivision : InformalIntermediateLevelSubdivision, IntermediateLevelSubdivisionToCreate
{
}
public interface InformalIntermediateLevelSubdivision: IntermediateLevelSubdivision
{
}
