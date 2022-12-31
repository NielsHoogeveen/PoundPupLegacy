namespace PoundPupLegacy.Db.Writers;

internal class SingleIdWriter : DatabaseWriter<int>
{

    internal const string ID = "id";

    internal static NpgsqlCommand CreateSingleIdCommand(string tableName, NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal static DatabaseWriter<int> CreateSingleIdWriter(string tableName, NpgsqlConnection connection)
    {
        return new SingleIdWriter(CreateSingleIdCommand(tableName, connection));

    }

    private SingleIdWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(int id)
    {
        WriteValue(id, ID);
        _command.ExecuteNonQuery();
    }
}
internal class SingleIdWriter<T> : DatabaseWriter<T> where T : Identifiable
{
    internal SingleIdWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(T node)
    {
        WriteValue(node.Id, SingleIdWriter.ID);
        _command.ExecuteNonQuery();
    }
}
