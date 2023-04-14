namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = SenatorSenateBillActionInserterFactory;
using Request = SenatorSenateBillAction;
using Inserter = SenatorSenateBillActionInserter;

internal sealed class SenatorSenateBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SenateBillId = new() { Name = "senate_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override string TableName => "senator_senate_bill_action";

}
internal sealed class SenatorSenateBillActionInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{
    public SenatorSenateBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command, generateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.SenatorId, request.SenatorId),
            ParameterValue.Create(Factory.SenateBillId, request.SenateBillId),
            ParameterValue.Create(Factory.Date, request.Date),
            ParameterValue.Create(Factory.BillActionTypeId, request.BillActionTypeId)
        };
    }
}
