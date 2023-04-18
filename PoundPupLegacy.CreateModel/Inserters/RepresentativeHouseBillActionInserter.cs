namespace PoundPupLegacy.CreateModel.Inserters;

using Request = RepresentativeHouseBillAction;

internal sealed class RepresentativeHouseBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter HouseBillId = new() { Name = "house_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

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
