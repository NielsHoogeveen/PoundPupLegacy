namespace PoundPupLegacy.Db.Writers;

internal class SingleIdWriter : DatabaseWriter<int>
{

    internal const string ID = "id";

    internal static async Task<NpgsqlCommand> CreateSingleIdCommandAsync(string tableName, NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            tableName,
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return command;
    }

    internal static async Task<DatabaseWriter<int>> CreateSingleIdWriterAsync(string tableName, NpgsqlConnection connection)
    {
        var command = await CreateSingleIdCommandAsync(tableName, connection);
        return new SingleIdWriter(command);

    }

    private SingleIdWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(int id)
    {
        WriteValue(id, ID);
        await _command.ExecuteNonQueryAsync();
    }
}
internal class SingleIdWriter<T> : DatabaseWriter<T> where T : Identifiable
{
    internal SingleIdWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(T node)
    {
        if (node.Id is null)
            throw new NullReferenceException();
        WriteValue(node.Id, SingleIdWriter.ID);
        await _command.ExecuteNonQueryAsync();
    }
}
