﻿namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewSenatorSenateBillAction;

internal sealed class SenatorSenateBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    private static readonly NonNullableIntegerDatabaseParameter SenateBillId = new() { Name = "senate_bill_id" };
    private static readonly NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    private static readonly NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override string TableName => "senator_senate_bill_action";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(SenatorId, request.SenatorId),
            ParameterValue.Create(SenateBillId, request.SenateBillId),
            ParameterValue.Create(Date, request.Date),
            ParameterValue.Create(BillActionTypeId, request.BillActionTypeId)
        };
    }
}
