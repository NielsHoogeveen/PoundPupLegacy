﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = BillToCreate;

internal sealed class BillInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableDateTimeDatabaseParameter IntroductionDate = new() { Name = "introduction_date" };
    private static readonly NullableIntegerDatabaseParameter ActId = new() { Name = "act_id" };

    public override string TableName => "bill";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(IntroductionDate, request.BillDetails.IntroductionDate),
            ParameterValue.Create(ActId, request.BillDetails.ActId)
        };
    }
}