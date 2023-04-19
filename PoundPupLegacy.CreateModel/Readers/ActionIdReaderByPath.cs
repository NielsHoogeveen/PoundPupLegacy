namespace PoundPupLegacy.CreateModel.Readers;

using Request = ActionIdReaderByPathRequest;

public sealed record ActionIdReaderByPathRequest : IRequest
{
    public required string Path { get; init; }
}

internal sealed class ActionIdReaderByPathFactory : IntDatabaseReaderFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Path = new() { Name = "path" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    protected override string GetErrorMessage(Request request)
    {
        return $"action {request} cannot be found";
    }

    protected override IntValueReader IntValueReader => IdReader;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        if (request is null) {
            throw new ArgumentNullException(nameof(request.Path));
        }
        return new ParameterValue[] {
            ParameterValue.Create(Path, request.Path)
        };
    }
    public override string Sql => SQL;

    private const string SQL = """
        SELECT id FROM basic_action WHERE path = @path
        """;
}

