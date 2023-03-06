namespace PoundPupLegacy.Db.Writers;

internal sealed class EditOwnNodeActionWriter : DatabaseWriter<EditOwnNodeAction>, IDatabaseWriter<EditOwnNodeAction>
{

    private const string ID = "id";
    private const string NODE_TYPE_ID = "node_type_id";
    public static async Task<DatabaseWriter<EditOwnNodeAction>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "edit_own_node_action",
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
        return new EditOwnNodeActionWriter(command);

    }

    internal EditOwnNodeActionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(EditOwnNodeAction editOwnNodeAction)
    {
        if (!editOwnNodeAction.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(editOwnNodeAction.Id.Value, ID);
        WriteNullableValue(editOwnNodeAction.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
