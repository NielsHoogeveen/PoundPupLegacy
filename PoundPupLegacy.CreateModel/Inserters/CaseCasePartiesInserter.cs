namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CaseCasePartiesInserterFactory;
using Request = CaseCaseParties;
using Inserter = CaseCasePartiesInserter;

internal sealed class CaseCasePartiesInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter CaseId = new() { Name = "case_id" };
    internal static NullCheckingIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override string TableName => "case_case_parties";
}
internal sealed class CaseCasePartiesInserter : DatabaseInserter<Request>
{

    public CaseCasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CaseId, request.CaseId),
            ParameterValue.Create(Factory.CasePartiesId, request.CaseParties?.Id),
            ParameterValue.Create(Factory.CasePartyTypeId, request.CasePartyTypeId)
        };
    }
}
