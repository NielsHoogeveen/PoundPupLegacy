namespace PoundPupLegacy.CreateModel.Readers;

using Factory = ActionIdReaderByPathFactory;
using Reader = ActionIdReaderByPath;

public sealed class ActionIdReaderByPathFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    public override string Sql => SQL;

    private const string SQL = """
        SELECT id FROM basic_action WHERE path = @path
        """;
}

public sealed class ActionIdReaderByPath : IntDatabaseReader<string>
{

    internal ActionIdReaderByPath(NpgsqlCommand command) : base(command) { }

    protected override string GetErrorMessage(string request)
    {
        return $"action {request} cannot be found";
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override IEnumerable<ParameterValue> GetParameterValues(string request)
    {
        if (request is null) {
            throw new ArgumentNullException(nameof(request));
        }
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Path, request)
        };
    }
}
