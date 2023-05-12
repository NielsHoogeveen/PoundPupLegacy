namespace PoundPupLegacy.CreateModel.Inserters;

using Request = AuthoringStatus;

internal sealed class AuthoringStatusInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "authoring_status";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
        };
    }
}
