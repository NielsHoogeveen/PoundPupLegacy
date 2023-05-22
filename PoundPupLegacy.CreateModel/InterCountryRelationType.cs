﻿namespace PoundPupLegacy.CreateModel;

public sealed record NewInterCountryRelationType : NewEndoRelationTypeBase, EventuallyIdentifiableInterCountryRelationType
{
}
public sealed record ExistingInterCountryRelationType : ExistingEndoRelationTypeBase, ImmediatelyIdentifiableInterCountryRelationType
{
}
public interface ImmediatelyIdentifiableInterCountryRelationType : InterCountryRelationType, ImmediatelyIdentifiableEndoRelationType
{
}
public interface EventuallyIdentifiableInterCountryRelationType : InterCountryRelationType, EventuallyIdentifiableEndoRelationType
{
}
public interface InterCountryRelationType : EndoRelationType
{
}