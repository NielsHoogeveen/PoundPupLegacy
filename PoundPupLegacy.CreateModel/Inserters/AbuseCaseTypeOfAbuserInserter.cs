namespace PoundPupLegacy.CreateModel.Inserters;

using Request = AbuseCaseTypeOfAbuser;

internal sealed class AbuseCaseTypeOfAbuserInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter AbuseCaseId = new() { Name = "abuse_case_id" };
    private static readonly NonNullableIntegerDatabaseParameter TypeOfAbuserId = new() { Name = "type_of_abuser_id" };
    public override string TableName => "abuse_case_type_of_abuser";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(AbuseCaseId, request.AbuseCaseId),
            ParameterValue.Create(TypeOfAbuserId, request.TypeOfAbuserId)
        };
    }
}
