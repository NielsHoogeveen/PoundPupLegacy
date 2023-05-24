﻿namespace PoundPupLegacy.CreateModel;

public sealed record NewDeportationCase : NewCaseBase, EventuallyIdentifiableDeportationCase
{
    public required int? SubdivisionIdFrom { get; init; }
    public required int? CountryIdTo { get; init; }
}
public sealed record ExistingDeportationCase : ExistingCaseBase, ImmediatelyIdentifiableDeportationCase
{
    public required int? SubdivisionIdFrom { get; init; }
    public required int? CountryIdTo { get; init; }
}
public interface ImmediatelyIdentifiableDeportationCase : DeportationCase, ImmediatelyIdentifiableCase
{
}
public interface EventuallyIdentifiableDeportationCase : DeportationCase, EventuallyIdentifiableCase
{
}
public interface DeportationCase: Case
{
    int? SubdivisionIdFrom { get; }
    int? CountryIdTo { get; }
}
