namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CaseTypeCasePartyTypeInserterFactory;
using Request = CaseTypeCasePartyType;
using Inserter = CaseTypeCasePartyTypeInserter;

internal sealed class CaseTypeCasePartyTypeInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter CaseTypeId = new() { Name = "case_type_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override string TableName => "case_type_case_party_type";

}
internal sealed class CaseTypeCasePartyTypeInserter : DatabaseInserter<Request>
{
    public CaseTypeCasePartyTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CaseTypeId, request.CaseTypeId),
            ParameterValue.Create(Factory.CasePartyTypeId, request.CasePartyTypeId),
        };
    }
}
