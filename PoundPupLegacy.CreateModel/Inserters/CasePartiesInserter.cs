namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CasePartiesInserterFactory;
using Request = CaseParties;
using Inserter = CasePartiesInserter;

internal sealed class CasePartiesInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableStringDatabaseParameter Organizations = new() { Name = "organizations" };
    internal static NullableStringDatabaseParameter Persons = new() { Name = "persons" };
    public override string TableName => "case_parties";
}
internal sealed class CasePartiesInserter : AutoGenerateIdDatabaseInserter<Request>
{
    public CasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Organizations, request.Organizations),
            ParameterValue.Create(Factory.Persons, request.Persons)
        };
    }
}
