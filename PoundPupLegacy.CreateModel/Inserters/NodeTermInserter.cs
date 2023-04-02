using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PoundPupLegacy.Db.Test")]
namespace PoundPupLegacy.CreateModel.Inserters;

public sealed class NodeTermInserter : DatabaseInserter<NodeTerm>, IDatabaseInserter<NodeTerm>
{
    private const string NODE_ID = "node_id";
    private const string TERM_ID = "term_id";
    public static async Task<DatabaseInserter<NodeTerm>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NODE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TERM_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new NodeTermInserter(command);

    }

    internal NodeTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(NodeTerm nodeTerm)
    {
        WriteValue(nodeTerm.NodeId, NODE_ID);
        WriteValue(nodeTerm.TermId, TERM_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
