namespace PoundPupLegacy.CreateModel.Inserters;

using Request = AbuseCaseTypeOfAbuse;

internal sealed class AbuseCaseTypeOfAbuseInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter AbuseCaseId = new() { Name = "abuse_case_id" };
    private static readonly NonNullableIntegerDatabaseParameter TypeOfAbuseId = new() { Name = "type_of_abuse_id" };
    public override string TableName => "abuse_case_type_of_abuse";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(AbuseCaseId, request.AbuseCaseId),
            ParameterValue.Create(TypeOfAbuseId, request.TypeOfAbuseId)
        };
    }
}
