namespace PoundPupLegacy.CreateModel.Readers;
public sealed class DeleteNodeActionIdReaderByNodeTypeIdFactory : DatabaseReaderFactory<DeleteNodeActionIdReaderByNodeTypeId>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    public override string Sql => SQL;
    
    private const string SQL = """
        SELECT id FROM delete_node_action WHERE node_type_id = @node_type_id
        """;
}
public sealed class DeleteNodeActionIdReaderByNodeTypeId : SingleItemDatabaseReader<int, int>
{

    internal DeleteNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(int nodeTypeId)
    {
        _command.Parameters["node_type_id"].Value = nodeTypeId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"delete node action cannot be found for node type  {nodeTypeId}");
    }
}
