namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DeportationCaseInserterFactory : DatabaseInserterFactory<DeportationCase, DeportationCaseInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter SubdivisionIdFrom = new() { Name = "subdivision_id_from" };
    internal static NullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };

    public override string TableName => "deportation_case";

}

internal sealed class DeportationCaseInserter : DatabaseInserter<DeportationCase>
{

    public DeportationCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(DeportationCase item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(DeportationCaseInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(DeportationCaseInserterFactory.SubdivisionIdFrom, item.SubdivisionIdFrom),
            ParameterValue.Create(DeportationCaseInserterFactory.CountryIdTo, item.CountryIdTo),
        };
    }
}
