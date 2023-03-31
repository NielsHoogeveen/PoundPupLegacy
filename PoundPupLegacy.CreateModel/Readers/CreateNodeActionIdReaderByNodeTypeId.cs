﻿namespace PoundPupLegacy.CreateModel.Readers;
public sealed class CreateNodeActionIdReaderByNodeTypeIdFactory : IDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeId>
{
    public async Task<CreateNodeActionIdReaderByNodeTypeId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id FROM create_node_action WHERE node_type_id = @node_type_id
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;

        command.CommandText = sql;
        command.Parameters.Add("node_type_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new CreateNodeActionIdReaderByNodeTypeId(command);

    }

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