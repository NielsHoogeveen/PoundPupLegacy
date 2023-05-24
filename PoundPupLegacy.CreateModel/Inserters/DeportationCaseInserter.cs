namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EventuallyIdentifiableDeportationCase;

internal sealed class DeportationCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableIntegerDatabaseParameter SubdivisionIdFrom = new() { Name = "subdivision_id_from" };
    private static readonly NullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };

    public override string TableName => "deportation_case";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(SubdivisionIdFrom, request.SubdivisionIdFrom),
            ParameterValue.Create(CountryIdTo, request.CountryIdTo),
        };
    }
}

