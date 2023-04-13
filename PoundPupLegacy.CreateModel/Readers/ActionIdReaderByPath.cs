namespace PoundPupLegacy.CreateModel.Readers;

using Request = ActionIdReaderByPathRequest;
using Factory = ActionIdReaderByPathFactory;
using Reader = ActionIdReaderByPath;

public sealed record ActionIdReaderByPathRequest: IRequest
{
    public required string Path { get; init; }
}

internal sealed class ActionIdReaderByPathFactory : MandatorySingleItemDatabaseReaderFactory<Request, int, Reader>
{
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    public override string Sql => SQL;

    private const string SQL = """
        SELECT id FROM basic_action WHERE path = @path
        """;
}

internal sealed class ActionIdReaderByPath : IntDatabaseReader<Request>
{
    public ActionIdReaderByPath(NpgsqlCommand command) : base(command) { }

    protected override string GetErrorMessage(Request request)
    {
        return $"action {request} cannot be found";
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        if (request is null) {
            throw new ArgumentNullException(nameof(request.Path));
        }
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Path, request.Path)
        };
    }
}
