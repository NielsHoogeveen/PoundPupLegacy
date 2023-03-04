namespace PoundPupLegacy.Db.Writers;

internal sealed class EditNodeActionWriter : DatabaseWriter<EditNodeAction>, IDatabaseWriter<EditNodeAction>
{

    private const string ID = "id";
    private const string NODE_TYPE_ID = "node_type_id";
    public static async Task<DatabaseWriter<EditNodeAction>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "edit_node_action",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NODE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new EditNodeActionWriter(command);

    }

    internal EditNodeActionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(EditNodeAction editNodeAccessPrivilege)
    {
        if (!editNodeAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(editNodeAccessPrivilege.Id.Value, ID);
        WriteNullableValue(editNodeAccessPrivilege.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
