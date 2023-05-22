namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewRepresentativeHouseBillAction;

internal sealed class RepresentativeHouseBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    private static readonly NonNullableIntegerDatabaseParameter HouseBillId = new() { Name = "house_bill_id" };
    private static readonly NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    private static readonly NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override string TableName => "representative_house_bill_action";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(RepresentativeId, request.RepresentativeId),
            ParameterValue.Create(HouseBillId, request.HouseBillId),
            ParameterValue.Create(Date, request.Date),
            ParameterValue.Create(BillActionTypeId, request.BillActionTypeId)
        };
    }
}
