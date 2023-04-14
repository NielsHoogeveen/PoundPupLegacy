namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SenatorSenateBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<SenatorSenateBillAction, SenatorSenateBillActionInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SenateBillId = new() { Name = "senate_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override string TableName => "senator_senate_bill_action";

}
internal sealed class SenatorSenateBillActionInserter : ConditionalAutoGenerateIdDatabaseInserter<SenatorSenateBillAction>
{
    public SenatorSenateBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command, generateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(SenatorSenateBillAction item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(SenatorSenateBillActionInserterFactory.Id, item.Id),
            ParameterValue.Create(SenatorSenateBillActionInserterFactory.SenatorId, item.SenatorId),
            ParameterValue.Create(SenatorSenateBillActionInserterFactory.SenateBillId, item.SenateBillId),
            ParameterValue.Create(SenatorSenateBillActionInserterFactory.Date, item.Date),
            ParameterValue.Create(SenatorSenateBillActionInserterFactory.BillActionTypeId, item.BillActionTypeId)
        };
    }
}
