namespace PoundPupLegacy.DomainModel.Inserters;

using Request = BasicAction;
internal sealed class BasicActionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    public override string TableName => "basic_action";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Path, request.Path),
            ParameterValue.Create(Description, request.Description)
        };
    }
}
