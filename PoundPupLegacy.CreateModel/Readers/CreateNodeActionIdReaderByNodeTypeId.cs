namespace PoundPupLegacy.CreateModel.Readers;
public sealed class CreateNodeActionIdReaderByNodeTypeIdFactory : DatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeId>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    public override string Sql => SQL;

    private const string SQL = @"
        SELECT id FROM create_node_action WHERE node_type_id = @node_type_id
        ";
}
public sealed class CreateNodeActionIdReaderByNodeTypeId : SingleItemDatabaseReader<int, int>
{

    internal CreateNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

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
        throw new Exception($"create node action cannot be found for node type  {nodeTypeId}");
    }
}
