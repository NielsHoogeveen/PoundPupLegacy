namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = BoundCountryInserterFactory;
using Request = BoundCountry;
using Inserter = BoundCountryInserter;

internal sealed class BoundCountryInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter BindingCountryId = new() { Name = "binding_country_id" };

    public override string TableName => "bound_country";
}
internal sealed class BoundCountryInserter : IdentifiableDatabaseInserter<Request>
{
    public BoundCountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.BindingCountryId, request.BindingCountryId)
        };
    }
}
