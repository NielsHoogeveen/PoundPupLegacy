﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CreateNodeActionInserter : DatabaseInserter<CreateNodeAction>, IDatabaseInserter<CreateNodeAction>
{

    private const string ID = "id";
    private const string NODE_TYPE_ID = "node_type_id";
    public static async Task<DatabaseInserter<CreateNodeAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "create_node_action",
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
        return new CreateNodeActionInserter(command);

    }

    internal CreateNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CreateNodeAction createNodeAccessPrivilege)
    {
        if (!createNodeAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(createNodeAccessPrivilege.Id.Value, ID);
        WriteNullableValue(createNodeAccessPrivilege.NodeTypeId, NODE_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
