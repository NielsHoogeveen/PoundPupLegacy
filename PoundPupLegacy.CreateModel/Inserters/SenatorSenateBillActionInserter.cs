namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SenatorSenateBillAction;

internal sealed class SenatorSenateBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SenateBillId = new() { Name = "senate_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

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
