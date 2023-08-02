﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = PollToCreate;

internal sealed class PollInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableDateTimeDatabaseParameter DateTimeClosure = new() { Name = "date_time_closure" };
    private static readonly NonNullableIntegerDatabaseParameter PollStatusId = new() { Name = "poll_status_id" };

    public override string TableName => "poll";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DateTimeClosure, request.PollDetails.DateTimeClosure),
            ParameterValue.Create(PollStatusId, request.PollDetails.PollStatusId),
        };
    }
}