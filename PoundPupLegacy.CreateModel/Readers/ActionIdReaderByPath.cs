namespace PoundPupLegacy.CreateModel.Readers;

public sealed class ActionIdReaderByPathFactory : DatabaseReaderFactory<ActionIdReaderByPath>
{
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    public override string Sql => SQL;

    private const string SQL = """
        SELECT id FROM basic_action WHERE path = @path
        """;
}
public sealed class ActionIdReaderByPath : SingleItemDatabaseReader<string, int>
{

    internal ActionIdReaderByPath(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(string path)
    {
        if (path is null) {
            throw new ArgumentNullException(nameof(path));
        }
        _command.Parameters["path"].Value = path;

        using var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        throw new Exception($"action {path} cannot be found");
    }
}
