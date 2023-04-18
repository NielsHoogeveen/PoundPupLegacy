namespace PoundPupLegacy.CreateModel.Inserters;

using Request = ChildTraffickingCase;

internal sealed class ChildTraffickingCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };

    public override string TableName => "child_trafficking_case";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NumberOfChildrenInvolved, request.NumberOfChildrenInvolved),
            ParameterValue.Create(CountryIdFrom, request.CountryIdFrom),
        };
    }
}
