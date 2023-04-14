namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class RepresentativeHouseBillActionInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<RepresentativeHouseBillAction, RepresentativeHouseBillActionInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter HouseBillId = new() { Name = "house_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override string TableName => "representative_house_bill_action";

}
internal sealed class RepresentativeHouseBillActionInserter : ConditionalAutoGenerateIdDatabaseInserter<RepresentativeHouseBillAction>
{
    public RepresentativeHouseBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command, generateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(RepresentativeHouseBillAction item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(RepresentativeHouseBillActionInserterFactory.Id, item.Id),
            ParameterValue.Create(RepresentativeHouseBillActionInserterFactory.RepresentativeId, item.RepresentativeId),
            ParameterValue.Create(RepresentativeHouseBillActionInserterFactory.HouseBillId, item.HouseBillId),
            ParameterValue.Create(RepresentativeHouseBillActionInserterFactory.Date, item.Date),
            ParameterValue.Create(RepresentativeHouseBillActionInserterFactory.BillActionTypeId, item.BillActionTypeId)
        };
    }
}
