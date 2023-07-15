namespace PoundPupLegacy.DomainModel.Inserters;

using Request = BoundCountry.ToCreate;

internal sealed class BoundCountryInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter BindingCountryId = new() { Name = "binding_country_id" };

    public override string TableName => "bound_country";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(BindingCountryId, request.BoundCountryDetails.BindingCountryId)
        };
    }
}
