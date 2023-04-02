namespace PoundPupLegacy.CreateModel.Readers;
public sealed class DeleteNodeActionIdReaderByNodeTypeIdFactory : IDatabaseReaderFactory<DeleteNodeActionIdReaderByNodeTypeId>
{
    public async Task<DeleteNodeActionIdReaderByNodeTypeId> CreateAsync(IDbConnection connection)
    {
        var sql = """
            SELECT id FROM delete_node_action WHERE node_type_id = @node_type_id
            """;

        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("node_type_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new DeleteNodeActionIdReaderByNodeTypeId(command);

    }

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
