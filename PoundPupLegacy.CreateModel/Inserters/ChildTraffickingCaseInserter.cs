namespace PoundPupLegacy.CreateModel.Inserters;

using Request = ChildTraffickingCase.ChildTraffickingCaseToCreate;

internal sealed class ChildTraffickingCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    private static readonly NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };

    public override string TableName => "child_trafficking_case";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NumberOfChildrenInvolved, request.ChildTraffickingCaseDetails.NumberOfChildrenInvolved),
            ParameterValue.Create(CountryIdFrom, request.ChildTraffickingCaseDetails.CountryIdFrom),
        };
    }
}
