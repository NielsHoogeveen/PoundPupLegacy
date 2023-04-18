namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CaseParties;

internal sealed class CasePartiesInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NullableStringDatabaseParameter Organizations = new() { Name = "organizations" };
    internal static NullableStringDatabaseParameter Persons = new() { Name = "persons" };
    public override string TableName => "case_parties";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Organizations, request.Organizations),
            ParameterValue.Create(Persons, request.Persons)
        };
    }
}
