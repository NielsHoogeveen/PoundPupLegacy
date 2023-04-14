namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = DeportationCaseInserterFactory;
using Request = DeportationCase;
using Inserter = DeportationCaseInserter;

internal sealed class DeportationCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableIntegerDatabaseParameter SubdivisionIdFrom = new() { Name = "subdivision_id_from" };
    internal static NullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };

    public override string TableName => "deportation_case";

}

internal sealed class DeportationCaseInserter : IdentifiableDatabaseInserter<Request>
{

    public DeportationCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.SubdivisionIdFrom, request.SubdivisionIdFrom),
            ParameterValue.Create(Factory.CountryIdTo, request.CountryIdTo),
        };
    }
}
