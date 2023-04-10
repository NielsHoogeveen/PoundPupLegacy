namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ChildTraffickingCaseInserterFactory : BasicDatabaseInserterFactory<ChildTraffickingCase, ChildTraffickingCaseInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };

    public override string TableName => "child_trafficking_case";
}

internal sealed class ChildTraffickingCaseInserter : BasicDatabaseInserter<ChildTraffickingCase>
{
    public ChildTraffickingCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(ChildTraffickingCase item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(ChildTraffickingCaseInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(ChildTraffickingCaseInserterFactory.NumberOfChildrenInvolved, item.NumberOfChildrenInvolved),
            ParameterValue.Create(ChildTraffickingCaseInserterFactory.CountryIdFrom, item.CountryIdFrom),
        };
    }
}
