﻿namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SubdivisionToCreate;

internal sealed class SubdivisionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    private static readonly NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override string TableName => "subdivision";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.SubdivisionDetails.Name),
            ParameterValue.Create(CountryId, request.SubdivisionDetails.CountryId),
            ParameterValue.Create(SubdivisionTypeId, request.SubdivisionDetails.SubdivisionTypeId),
        };
    }
}
