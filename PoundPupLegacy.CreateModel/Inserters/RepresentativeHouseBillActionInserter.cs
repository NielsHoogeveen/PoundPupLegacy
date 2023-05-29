namespace PoundPupLegacy.CreateModel.Inserters;

using Request = RepresentativeHouseBillAction;

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
            ParameterValue.Create(RepresentativeId, request.RepresentativeHouseBillActionDetails.RepresentativeId),
            ParameterValue.Create(HouseBillId, request.RepresentativeHouseBillActionDetails.HouseBillId),
            ParameterValue.Create(Date, request.RepresentativeHouseBillActionDetails.Date),
            ParameterValue.Create(BillActionTypeId, request.RepresentativeHouseBillActionDetails.BillActionTypeId)
        };
    }
}
