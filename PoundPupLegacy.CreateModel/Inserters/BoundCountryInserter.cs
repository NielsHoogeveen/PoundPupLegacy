namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BoundCountryInserterFactory : DatabaseInserterFactory<BoundCountry, BoundCountryInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter BindingCountryId = new() { Name = "binding_country_id" };

    public override string TableName => "bound_country";
}
internal sealed class BoundCountryInserter : DatabaseInserter<BoundCountry>
{
    public BoundCountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(BoundCountry item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(BoundCountryInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(BoundCountryInserterFactory.BindingCountryId, item.BindingCountryId)
        };
    }
}
