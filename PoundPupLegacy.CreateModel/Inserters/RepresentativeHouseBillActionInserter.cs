namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = RepresentativeHouseBillActionInserterFactory;
using Request = RepresentativeHouseBillAction;
using Inserter = RepresentativeHouseBillActionInserter;

internal sealed class RepresentativeHouseBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter HouseBillId = new() { Name = "house_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override string TableName => "representative_house_bill_action";

}
internal sealed class RepresentativeHouseBillActionInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{
    public RepresentativeHouseBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command, generateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.RepresentativeId, request.RepresentativeId),
            ParameterValue.Create(Factory.HouseBillId, request.HouseBillId),
            ParameterValue.Create(Factory.Date, request.Date),
            ParameterValue.Create(Factory.BillActionTypeId, request.BillActionTypeId)
        };
    }
}
