namespace PoundPupLegacy.CreateModel.Readers;

public sealed class ActionIdReaderByPathFactory : IDatabaseReaderFactory<ActionIdReaderByPath>
{
    public async Task<ActionIdReaderByPath> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id FROM basic_action WHERE path = @path
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("path", NpgsqlDbType.Varchar);
        await command.PrepareAsync();

        return new ActionIdReaderByPath(command);

    }
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
