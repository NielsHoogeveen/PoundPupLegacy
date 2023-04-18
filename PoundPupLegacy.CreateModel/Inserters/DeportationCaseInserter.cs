namespace PoundPupLegacy.CreateModel.Inserters;

using Request = DeportationCase;

internal sealed class DeportationCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullableIntegerDatabaseParameter SubdivisionIdFrom = new() { Name = "subdivision_id_from" };
    internal static NullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };

    public override string TableName => "deportation_case";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(SubdivisionIdFrom, request.SubdivisionIdFrom),
            ParameterValue.Create(CountryIdTo, request.CountryIdTo),
        };
    }
}

