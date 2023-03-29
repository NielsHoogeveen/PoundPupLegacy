namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class EditNodeActionInserter : DatabaseInserter<EditNodeAction>, IDatabaseInserter<EditNodeAction>
{

    private const string ID = "id";
    private const string NODE_TYPE_ID = "node_type_id";
    public static async Task<DatabaseInserter<EditNodeAction>> CreateAsync(NpgsqlConnection connection)
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
        return new EditNodeActionInserter(command);

    }

    internal EditNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(EditNodeAction editNodeAction)
    {
        if (!editNodeAction.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(editNodeAction.Id.Value, ID);
        WriteNullableValue(editNodeAction.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
