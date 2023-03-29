namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class EditOwnNodeActionInserter : DatabaseInserter<EditOwnNodeAction>, IDatabaseInserter<EditOwnNodeAction>
{

    private const string ID = "id";
    private const string NODE_TYPE_ID = "node_type_id";
    public static async Task<DatabaseInserter<EditOwnNodeAction>> CreateAsync(NpgsqlConnection connection)
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
        return new EditOwnNodeActionInserter(command);

    }

    internal EditOwnNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(EditOwnNodeAction editOwnNodeAction)
    {
        if (!editOwnNodeAction.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(editOwnNodeAction.Id.Value, ID);
        WriteNullableValue(editOwnNodeAction.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
