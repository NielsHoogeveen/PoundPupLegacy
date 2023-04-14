namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = ChildTraffickingCaseInserterFactory;
using Request = ChildTraffickingCase;
using Inserter = ChildTraffickingCaseInserter;

internal sealed class ChildTraffickingCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };

    public override string TableName => "child_trafficking_case";
}

internal sealed class ChildTraffickingCaseInserter : IdentifiableDatabaseInserter<Request>
{
    public ChildTraffickingCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NumberOfChildrenInvolved, request.NumberOfChildrenInvolved),
            ParameterValue.Create(Factory.CountryIdFrom, request.CountryIdFrom),
        };
    }
}
