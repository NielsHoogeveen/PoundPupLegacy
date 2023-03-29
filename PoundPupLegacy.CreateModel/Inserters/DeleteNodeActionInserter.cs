namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DeleteNodeActionInserter : DatabaseInserter<DeleteNodeAction>, IDatabaseInserter<DeleteNodeAction>
{

    private const string ID = "id";
    private const string NODE_TYPE_ID = "node_type_id";
    public static async Task<DatabaseInserter<DeleteNodeAction>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "delete_node_action",
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
        return new DeleteNodeActionInserter(command);

    }

    internal DeleteNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DeleteNodeAction deleteNodeAccessPrivilege)
    {
        if (!deleteNodeAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(deleteNodeAccessPrivilege.Id.Value, ID);
        WriteNullableValue(deleteNodeAccessPrivilege.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
