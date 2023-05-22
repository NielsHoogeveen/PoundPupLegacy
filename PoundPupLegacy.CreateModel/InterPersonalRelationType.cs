﻿namespace PoundPupLegacy.CreateModel;

public sealed record NewInterPersonalRelationType : NewEndoRelationTypeBase, EventuallyIdentifiableInterPersonalRelationType
{
}
public sealed record ExistingInterPersonalRelationType : ExistingEndoRelationTypeBase, ImmediatelyIdentifiableInterPersonalRelationType
{
}
public interface ImmediatelyIdentifiableInterPersonalRelationType : InterPersonalRelationType, ImmediatelyIdentifiableEndoRelationType
{
}
public interface EventuallyIdentifiableInterPersonalRelationType : InterPersonalRelationType, EventuallyIdentifiableEndoRelationType
{
}
public interface InterPersonalRelationType : EndoRelationType
{
}
